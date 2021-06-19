using AutoMapper;
using Dapr.Client;
using Microsoft.EntityFrameworkCore;
using SmartEnergy.Contract.CustomExceptions;
using SmartEnergy.Contract.CustomExceptions.Call;
using SmartEnergy.Contract.CustomExceptions.Consumer;
using SmartEnergy.Contract.CustomExceptions.Device;
using SmartEnergy.Contract.CustomExceptions.DeviceUsage;
using SmartEnergy.Contract.CustomExceptions.Incident;
using SmartEnergy.Contract.CustomExceptions.Location;
using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Enums;
using SmartEnergy.Contract.Interfaces;
using SmartEnergy.Documents.DomainModels;
using SmartEnergy.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartEnergy.Documents.Services
{
    public class IncidentService : IIncidentService
    {

        private readonly DocumentsDbContext _dbContext;
        private readonly ITimeService _timeService;   
        private readonly ICallService _callService;
        private readonly IMapper _mapper;
        private readonly IAuthHelperService _authHelperService;
        private readonly DaprClient _daprClient;

        public IncidentService(DocumentsDbContext dbContext, ITimeService timeService, ICallService callService, IMapper mapper,
            IAuthHelperService authHelperService, DaprClient daprClient)
        {
            _dbContext = dbContext;
            _timeService = timeService;
            _callService = callService;
            _mapper = mapper;
            _authHelperService = authHelperService;
            _daprClient = daprClient;
        }




        // determine what to delete with incident object
        public void Delete(int id)
        {
            Incident incident = _dbContext.Incidents.Include(x => x.MultimediaAnchor)
                                                    .Include(x => x.NotificationAnchor)
                                                    .Include(x => x.WorkRequest)
                                                    .FirstOrDefault(x => x.ID == id);
            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {id} does not exist.");

            _dbContext.Incidents.Remove(incident);

           // Remove anchors
            _dbContext.MultimediaAnchors.Remove(incident.MultimediaAnchor);
            _dbContext.NotificationAnchors.Remove(incident.NotificationAnchor);
  

            _dbContext.SaveChanges();
        }

        public async Task< IncidentDto> GetAsync(int id)
        {
            Incident incident = _dbContext.Incidents.Find(id);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {id} does not exist.");

            return _mapper.Map<IncidentDto>(incident);
        }


        public async Task<List<IncidentDto>> GetAllAsync()
        {
            return _mapper.Map<List<IncidentDto>>(_dbContext.Incidents.ToList());
        }

        /// <summary>
        /// Get incident location
        /// </summary>
        /// <param name="incidentId"></param>
        /// <returns>LocationDto</returns>
        public async Task<LocationDto> GetIncidentLocationAsync(int incidentId)
        {
            //TODO:
            Incident incident = _dbContext.Incidents.FirstOrDefault(x => x.ID == incidentId);
            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");

            DayPeriod currentDayPeriod = _timeService.GetCurrentDayPeriod();
            try
            {
                List<DeviceDto> devices = await _daprClient.InvokeMethodAsync<List<DeviceDto>>(HttpMethod.Get, "smartenergyphysical", $"/api/devices/incident/{incident.ID}");
                //Try getting location from devices
                if (currentDayPeriod == DayPeriod.MORNING)
                    devices = devices.OrderByDescending(x => x.Location.MorningPriority).ToList();
                else if (currentDayPeriod == DayPeriod.NOON)
                    devices = devices.OrderByDescending(x => x.Location.NoonPriority).ToList();
                else
                    devices = devices.OrderByDescending(x => x.Location.NightPriority).ToList();

                foreach (DeviceDto d in devices)
                {
                    return d.Location;
                }
            }
            catch { }
            incident = _dbContext.Incidents.Include(x => x.Calls).FirstOrDefault(x => x.ID == incidentId);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");

            //Try getting location from calls
            foreach (Call c in incident.Calls)
            {
                try
                {
                    LocationDto location = await _daprClient.InvokeMethodAsync<LocationDto>(HttpMethod.Get, "smartenergyphysical", $"/api/locations/{c.LocationID}");
                    return location;
                }
                catch { }
            }

            throw new LocationNotFoundException($"Location does not exist for incident with id {incidentId}");
        }

        public async Task<IncidentDto> InsertAsync(IncidentDto entity)
        {
            ValidateIncident(entity);

            MultimediaAnchor mAnchor = new MultimediaAnchor();
   
            NotificationAnchor nAnchor = new NotificationAnchor();

            Incident incident = _mapper.Map<Incident>(entity);
            incident.ID = 0;
            incident.MultimediaAnchor = mAnchor;
            incident.NotificationAnchor = nAnchor;
            incident.Timestamp = DateTime.Now;


            if (incident.ETR != null)
                incident.ETR = incident.ETR.Value.AddHours(2);

            incident.Priority = 0;
            incident.IncidentStatus = IncidentStatus.INITIAL; // with basic info only init

        
            _dbContext.Incidents.Add(incident);

            _dbContext.SaveChanges();

            return _mapper.Map<IncidentDto>(incident);
        }

        public async Task<IncidentDto> UpdateAsync(IncidentDto entity)
        {
            ValidateIncident(entity);

            Incident oldIncident = _dbContext.Incidents.Find(entity.ID);

            if (oldIncident == null)
                throw new IncidentNotFoundException($"Incident with id {entity.ID} does not exist");


            Incident entityIncident = _mapper.Map<Incident>(entity);

            if (entityIncident.Timestamp < oldIncident.Timestamp)
                throw new InvalidIncidentException("You have tried to modify outdated incident. Please, try again.");

            if (entityIncident.ETR != null)
                entityIncident.ETR = entityIncident.ETR.Value.AddHours(2);

            oldIncident.Update(_mapper.Map<Incident>(entityIncident));

            _dbContext.SaveChanges();
            
            return _mapper.Map<IncidentDto>(oldIncident);

        }



        private void ValidateIncident(IncidentDto entity)
        {

            if (!Enum.IsDefined(typeof(WorkType), entity.WorkType))
                throw new InvalidIncidentException("Undefined work type!");

            if (!Enum.IsDefined(typeof(IncidentStatus), entity.IncidentStatus))
                throw new InvalidIncidentException("Undefined incident status!");

            if (entity.Description == null || entity.Description.Length > 100)
                throw new InvalidIncidentException($"Description must be at most 100 characters long!");


            if (entity.VoltageLevel <= 0)
                throw new InvalidIncidentException("Voltage level have to be greater than 0!");

            //proveriti validacije za datume

            if (entity.IncidentDateTime > entity.ETA)
                throw new InvalidIncidentException($"ETA date cannot be before incident date!");

            if (entity.IncidentDateTime > entity.ATA)
                throw new InvalidIncidentException($"ATA date cannot be before incident date!");

         

            if (entity.WorkBeginDate < entity.IncidentDateTime)
                throw new InvalidIncidentException($"Sheduled date cannot be before incident date.");




        }
       
        
        
        /// <summary>
        /// Get priority for specific incident by finding device related to incident with highest priority.
        /// </summary>
        /// <param name="incidentId"></param>
        /// <returns>Integer priority</returns>
        private async Task<int> GetIncidentPriorityAsync(int incidentId)
        {

            int priority = -1;

            Incident incident = _dbContext.Incidents.FirstOrDefault(x => x.ID == incidentId);
            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");


            List<int> allPriorities = new List<int>();

            DayPeriod currentDayPeriod = _timeService.GetCurrentDayPeriod();

            try
            {
                List<DeviceDto> devices = await _daprClient.InvokeMethodAsync<List<DeviceDto>>(HttpMethod.Get, "smartenergyphysical", $"/api/devices/incident/{incident.ID}");
                foreach (DeviceDto d in devices)
                {

                    if (currentDayPeriod == DayPeriod.MORNING)
                        allPriorities.Add(d.Location.MorningPriority);
                    else if (currentDayPeriod == DayPeriod.NOON)
                        allPriorities.Add(d.Location.NoonPriority);
                    else
                        allPriorities.Add(d.Location.NightPriority);
                }


            }
            catch { }


            if (allPriorities.Count != 0)
                priority = allPriorities.Max();
            else
                priority = 0;



            if (priority != -1)
                return priority;
            else
                return 0;

          

          
        }


        /// <summary>
        /// Connect specific crew with incident
        /// </summary>
        /// <param name="incidentId"></param>
        /// <param name="crewId"></param>
        /// <returns></returns>
        public async Task<IncidentDto> AddCrewToIncidentAsync(int incidentId, int crewId)
        {
            Incident incident = _dbContext.Incidents.Find(incidentId);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id = {incidentId} does not exists!");
            try
            {
                CrewDto crew = await _daprClient.InvokeMethodAsync<CrewDto>(HttpMethod.Get, "smartenergyusers", $"/api/crews/{crewId}");
                incident.CrewID = crew.ID;


                _dbContext.SaveChanges();

                return _mapper.Map<IncidentDto>(incident);
            }
            catch
            {
                throw new CrewNotFoundException($"Crew with id = {crewId} does not exists!");
            }


        }


        /// <summary>
        /// This function get all incidents which are ready for usage in work request
        /// </summary>
        /// <returns>List of ready incidents</returns>
        public List<IncidentDto> GetUnassignedIncidents()
        {

            bool isFree = true;

            List<Incident> allIncidents = _dbContext.Incidents.ToList();
            List<WorkRequest> allWorkRequests = _dbContext.WorkRequests.Include("Incident").ToList();

            List<Incident> unassignedIcidents = new List<Incident>();

            foreach(Incident incident in allIncidents)
            {
                isFree = true;

                foreach(WorkRequest workRequest in allWorkRequests)
                {
                    if (workRequest.IncidentID.Equals(incident.ID))
                    {
                        isFree = false;
                        break;
                    }
                   
                }

                if( !incident.IncidentStatus.Equals(IncidentStatus.INITIAL) && isFree)
                    unassignedIcidents.Add(incident);


            }


            
            return _mapper.Map<List<IncidentDto>>(unassignedIcidents);
        }

        public async void AddDeviceToIncidentAsync(int incidentId, int deviceId)
        {

            Incident incident = _dbContext.Incidents.FirstOrDefault(x => x.ID == incidentId);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");
            DeviceDto device = null;
            try
            {
                device = await _daprClient.InvokeMethodAsync<DeviceDto>(HttpMethod.Get, "smartenergyphysical", $"/api/devices/{deviceId}");
            }
            catch {
                throw new DeviceNotFoundException($"Device with id = { deviceId} does not exists!");
            }

            try
            {
                List<DeviceDto> incidentDevices = await _daprClient.InvokeMethodAsync<List<DeviceDto>>(HttpMethod.Get, "smartenergyphysical", $"/api/devices/incident/{incidentId}");
                if (incidentDevices.Count != 0)
                {
                    foreach (DeviceDto du in incidentDevices)
                    {
                        if (!CompareLocation(du.Location, device.Location))
                            throw new InvalidDeviceException($"Device has to be on {du.Location.Street}, {du.Location.City}, {du.Location.Zip}!");
                    }
                }
            }
            catch
            {
                throw new Exception("Device service unavailable");
            }

            
            List<Call> callWithoutIncident = _dbContext.Calls.Where(x => x.IncidentID == null).ToList();
            
            foreach(Call c in callWithoutIncident)
            {
                LocationDto callLocation = await _daprClient.InvokeMethodAsync<LocationDto>(HttpMethod.Get, "smartenergyphysical", $"/api/locations/{c.LocationID}");
                if(CompareLocation(callLocation, device.Location))
                {
                    c.IncidentID = incidentId;
                    await _callService.UpdateAsync(_mapper.Map<CallDto>(c));
                }

            }


            /*
            if (incident.IncidentDevices.Find(x => x.DeviceID == deviceId) != null)
                throw new InvalidDeviceUsageException($"Device with id = {deviceId} is already added to incident!");
            */

            var request = _daprClient.CreateInvokeMethodRequest<DeviceUsageDto>("smartenergyphysical", $"/api/devices/device-usage", new DeviceUsageDto { IncidentID = incidentId, DeviceID = deviceId });
            await _daprClient.InvokeMethodWithResponseAsync(request);

            incident.Priority = await GetIncidentPriorityAsync(incident.ID);
            
            if(incident.IncidentStatus == IncidentStatus.INITIAL)
                incident.IncidentStatus = IncidentStatus.UNRESOLVED;


            _dbContext.SaveChanges();

        }

        public IncidentDto RemoveCrewFromIncidet(int incidentId)
        {
            Incident incident = _dbContext.Incidents.Find(incidentId);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id = {incidentId} does not exists!");

            incident.CrewID = null;
            _dbContext.SaveChanges();


            return _mapper.Map<IncidentDto>(incident);
        }

        public async void RemoveDeviceFromIncindet(int incidentId, int deviceId)
        {
            Incident incident = _dbContext.Incidents
                                                   .FirstOrDefault(x => x.ID == incidentId);
            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");
            List<DeviceDto> incDevices = await _daprClient.InvokeMethodAsync<List<DeviceDto>>(HttpMethod.Get, "smartenergyphysical", $"/api/devices/incident/{incidentId}");
            DeviceDto device = incDevices.Find(x => x.ID == deviceId);

            if (device == null)
                throw new DeviceNotFoundException($"Device with id = { deviceId} does not exists!");

            await _daprClient.InvokeMethodAsync(HttpMethod.Delete, "smartenergyphysical", $"/api/devices/{deviceId}/incident/{incidentId}");

            incident.Priority = await GetIncidentPriorityAsync(incident.ID);


            _dbContext.SaveChanges();
        }

        public async Task<List<IncidentMapDisplayDto>> GetUnresolvedIncidentsForMapAsync()
        {
            List<Incident> incidents = _dbContext.Incidents
                                                           .Where(x => x.IncidentStatus == IncidentStatus.UNRESOLVED).ToList();
            List<IncidentMapDisplayDto> returnValue = new List<IncidentMapDisplayDto>();

            foreach(Incident incident in incidents)
            {
                try
                {
                    returnValue.Add(new IncidentMapDisplayDto()
                    {
                        ID = incident.ID,
                        IncidentDateTime = incident.IncidentDateTime,
                        Priority = incident.Priority,
                        Location = await GetIncidentLocationAsync(incident.ID),
                        Crew = await _daprClient.InvokeMethodAsync<CrewDto>(HttpMethod.Get, "smartenergyusers", $"/api/crews/{incident.CrewID}")
 
                    });
                }catch
                {

                }
            }

            return returnValue;
        }


     /// <summary>
     /// Get all calls for incident
     /// Compare location of devices assigned to incident and calls location
     /// </summary>
     /// <param name="incidentId"></param>
     /// <returns></returns>
        public async Task<List<CallDto>> GetIncidentCallsAsync(int incidentId)
        {

            List<DeviceDto> incidentDevices = await GetIncidentDevicesAsync(incidentId);
            List<CallDto> allCalls =  await _callService.GetAllAsync();

           

            if(incidentDevices.Count != 0)
            {

                foreach(CallDto c in allCalls)
                {
                    if(c.IncidentID == null && CompareLocation(c.Location, incidentDevices[0].Location))
                    {
                        c.IncidentID = incidentId;
                        await _callService.UpdateAsync(_mapper.Map<CallDto>(c));
                    }


                }

              
            }

            List<CallDto> calls = await _callService.GetAllAsync();
            calls = calls.FindAll(x => x.IncidentID == incidentId).ToList();

            return calls;


        }

        public async Task<int> GetNumberOfCallsAsync(int incidentId)
        {
            List<CallDto> calls = await _callService.GetAllAsync();
            return calls.Where(x => x.IncidentID == incidentId).Count();
        }

        public async Task<int> GetNumberOfAffectedConsumersAsync(int incidentId)
        {
            int affectedConsumers = 0;

            Incident incident = _dbContext.Incidents//.Include(x => x.IncidentDevices)
                                                  // .ThenInclude(p => p.Device)
                                                   //.ThenInclude(o => o.Location)
                                                   .FirstOrDefault(x => x.ID == incidentId);
            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");

            List<string> deviceStreets = new List<string>();
            try
            {


                List<DeviceDto> incidentDevices = await _daprClient.InvokeMethodAsync<List<DeviceDto>>(HttpMethod.Get, "smartenergyphysical", $"/api/devices/incident/{incidentId}");

                foreach (DeviceDto device in incidentDevices)
                {
                    if (!deviceStreets.Contains(device.Location.Street.ToLower().Trim()))
                        deviceStreets.Add(device.Location.Street.ToLower().Trim());

                }

                List<ConsumerDto> consumers = await _daprClient.InvokeMethodAsync<List<ConsumerDto>>(HttpMethod.Get, "smartenergyusers", $"/api/consumers");

                foreach (string deviceStreet in deviceStreets)
                {
                    foreach (ConsumerDto consumer in consumers)
                    {
                        consumer.Location = await _daprClient.InvokeMethodAsync<LocationDto>(HttpMethod.Get, "smartenergyphysical", $"/api/locations/{consumer.LocationID}");
                        if (consumer.Location.Street.ToLower().Trim().Equals(deviceStreet))
                            affectedConsumers++;
                    }
                }
            } catch (Exception e) { return 0; }


            return affectedConsumers;
            
        }

        public async Task<List<DeviceDto>> GetIncidentDevicesAsync(int incidentId)
        {
            Incident incident = _dbContext.Incidents
                                                   .FirstOrDefault(x => x.ID == incidentId);
            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");

            try
            {
                var ret = await _daprClient.InvokeMethodAsync<List<DeviceDto>>(HttpMethod.Get, "smartenergyphysical", $"/api/devices/incident/{incidentId}");
                return ret;
            }
            catch
            {
                return null;
            }
            

        }

        public async void SetIncidentPriority(int incidentId)
        {
            int incidentPriority = await GetIncidentPriorityAsync(incidentId);

            Incident incident = _dbContext.Incidents.Find(incidentId);

            incident.Priority = incidentPriority;
            _dbContext.SaveChanges();

        }

        public async Task<List<DeviceDto>> GetUnrelatedDevicesAsync(int incidentId)
        {

            
            Incident incident = _dbContext.Incidents//.Include(x => x.IncidentDevices)
                                                  //.ThenInclude(p => p.Device)
                                                  //.ThenInclude(o => o.Location)
                                                  .FirstOrDefault(x => x.ID == incidentId);
            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");


            List<int> incidentDeviceIds = new List<int>();
            List < DeviceDto > devices = await GetIncidentDevicesAsync(incidentId);



            foreach(DeviceDto device in devices)
            {
                 incidentDeviceIds.Add(device.ID);
            }

            bool condition = true;
            List<DeviceDto> devicesToReturn = new List<DeviceDto>();

            try
            {
                List<DeviceDto> allDevices = await _daprClient.InvokeMethodAsync<List<DeviceDto>>(HttpMethod.Get, "smartenergyphysical", $"/api/devices");

                foreach (DeviceDto d in allDevices)
                {
                    condition = true;
                    foreach (int deviceId in incidentDeviceIds)
                    {
                        if (d.ID == deviceId)
                        {
                            condition = false;
                            break;

                        }

                    }

                    if (condition)
                        devicesToReturn.Add(d);
                }

                return devicesToReturn;
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<CrewDto> GetIncidentCrewAsync(int incidentId)
        {
            Incident incident = _dbContext.Incidents
                                                    .FirstOrDefault(x => x.ID == incidentId);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");

            try
            {
                CrewDto crew = await _daprClient.InvokeMethodAsync<CrewDto>(HttpMethod.Get, "smartenergyusers", $"/api/crews/{incident.CrewID}");
                return crew;
            }
            catch
            {
                return null;
            }
        }

        private bool CompareLocation(LocationDto location1, LocationDto location2)
        {
            if( (location1.Zip == location2.Zip) &&
                (location1.Street.Equals(location2.Street))
                && (location1.City.Equals(location2.City)))
            {
                return true;
            }else
            {
                return false;
            }
                    
        }

        public async Task<CallDto> AddIncidentCallAsync(int incidentId, CallDto newCall)
        {

            Incident incident = _dbContext.Incidents//.Include(x => x.Crew)
                                                  //.ThenInclude(x => x.CrewMembers)
                                                  .FirstOrDefault(x => x.ID == incidentId);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exists.");


            ValidateCall(newCall);

            newCall.IncidentID = incidentId;
            return await _callService.InsertAsync(newCall);
        }


        private async void ValidateCall(CallDto entity)
        {
            if (entity.Hazard.Trim().Equals("") || entity.Hazard == null)
                throw new InvalidCallException("You have to enter call hazard!");

            if (!Enum.IsDefined(typeof(CallReason), entity.CallReason))
                throw new InvalidCallException("Undefined call reason!");

            try
            {
               await _daprClient.InvokeMethodAsync<LocationDto>(HttpMethod.Get, "smartenergyphysical", $"/api/locations/{entity.LocationID}");
            }
            catch
            {
                throw new LocationNotFoundException($"Location with id = {entity.LocationID} does not exists!");
            }
                


            if (entity.ConsumerID != 0 && entity.ConsumerID != null)  // ako nije anoniman
            {
                try
                {
                    await _daprClient.InvokeMethodAsync<ConsumerDto>(HttpMethod.Get, "smartenergyusers", $"/api/users/{entity.ConsumerID}");
                }
                catch
                {
                    throw new ConsumerNotFoundException($"Consumer with id = {entity.ConsumerID} does not exists!");
                }

            }
        }

        public void AssignIncidetToUser(int incidentId, int userId)
        {
            Incident incident = _dbContext.Incidents.Find(incidentId);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with id {incidentId} does not exist.");

            try
            {
                _daprClient.InvokeMethodAsync<UserDto>(HttpMethod.Get, "smartenergyusers", $"/api/users/{userId}");

            }
            catch
            {
                throw new UserNotFoundException($"User with id {userId} does not exist.");
            }

            incident.UserID = userId;


            int locationId = -1;

            //Skip sending mails for now

           /* if(incident.IncidentDevices.Count != 0)
            {
                locationId = incident.IncidentDevices[0].Device.LocationID;
            }

            List<Consumer> consumers = _dbContext.Consumers.Include(x => x.User).Where(x => x.LocationID == locationId).ToList();

            foreach(Consumer c in consumers)
            {
                _mailService.SendMail(c.User.Email, "Solving incident", "Problem will be solved as soon as possible. We are working!");
            }*/



            _dbContext.SaveChanges();


        }

        public IncidentListDto GetIncidentsPaged(IncidentFields sortBy, SortingDirection direction, int page, int perPage, IncidentFilter filter, OwnerFilter owner, string searchParam, ClaimsPrincipal user)
        {
            IQueryable<Incident> incidents = _dbContext.Incidents//.Include(x => x.User)
                                                                 .AsQueryable();

            incidents = FilterIncidents(incidents, filter);
            incidents = FilterIncidentsByOwner(incidents, owner, user);
            incidents = SearchIncidents(incidents, searchParam);
            incidents = SortIncidents(incidents, sortBy, direction);

            int resourceCount = incidents.Count();
            incidents = incidents.Skip(page * perPage)
                                    .Take(perPage);

            IncidentListDto returnValue = new IncidentListDto()
            {
                Incidents = _mapper.Map<List<IncidentDto>>(incidents.ToList()),
                TotalCount = resourceCount
            };

            return returnValue;
        }


        private IQueryable<Incident> FilterIncidents(IQueryable<Incident> incidents, IncidentFilter filter)
        {
            //Filter by status, ignore if ALL
            switch (filter)
            {
                case IncidentFilter.CONFIRMED:
                    return incidents.Where(x => x.Confirmed == true);
                case IncidentFilter.RESOLVED:
                    return incidents.Where(x => x.IncidentStatus == IncidentStatus.RESOLVED);
                case IncidentFilter.UNRESOLVED:
                    return incidents.Where(x => x.IncidentStatus == IncidentStatus.UNRESOLVED);
                case IncidentFilter.INITIAL:
                    return incidents.Where(x => x.IncidentStatus == IncidentStatus.INITIAL);
                case IncidentFilter.PLANNED:
                    return incidents.Where(x => x.WorkType == WorkType.PLANNED);
                case IncidentFilter.UNPLANNED:
                    return incidents.Where(x => x.WorkType == WorkType.UNPLANNED);

              
            }

            return incidents;
        }

        private IQueryable<Incident> FilterIncidentsByOwner(IQueryable<Incident> incidents, OwnerFilter owner, ClaimsPrincipal user)
        {
            int userId = _authHelperService.GetUserIDFromPrincipal(user);
            if (owner == OwnerFilter.mine)
                incidents = incidents.Where(x => x.UserID == userId);

            return incidents;
        }

        private IQueryable<Incident> SearchIncidents(IQueryable<Incident> incidents, string searchParam)
        {
            if (string.IsNullOrWhiteSpace(searchParam)) //Ignore empty search
                return incidents;
           
            return incidents.Where(x => x.ID.ToString().Trim().ToLower().Contains(searchParam.Trim().ToLower()) ||
                                        x.VoltageLevel.ToString().Trim().ToLower().Contains(searchParam.Trim().ToLower()) ||
                                        x.Priority.ToString().Trim().ToLower().Contains(searchParam.Trim().ToLower()));


        }

        private IQueryable<Incident> SortIncidents(IQueryable<Incident> incidents, IncidentFields sortBy, SortingDirection direction)
        {
            //Sort
            if (direction == SortingDirection.asc)
            {
                switch (sortBy)
                {
                    case IncidentFields.ID:
                        return incidents.OrderBy(x => x.ID);
                    case IncidentFields.ATA:
                        return incidents.OrderBy(x => x.ATA);
                    case IncidentFields.CONFIRMED:
                        return incidents.OrderBy(x => x.Confirmed);
                    case IncidentFields.ETA:
                        return incidents.OrderBy(x => x.ETA);
                    case IncidentFields.ETR:
                        return incidents.OrderBy(x => x.ETR.Value.Hour)
                                         .ThenBy(x => x.ETR.Value.Minute);

                    case IncidentFields.INCIDENTDATETIME:
                        return incidents.OrderBy(x => x.IncidentDateTime);
                    case IncidentFields.PLANNEDWORK:
                        return incidents.OrderBy(x => x.WorkBeginDate);
                    case IncidentFields.PRIORITY:
                        return incidents.OrderBy(x => x.Priority);
                    case IncidentFields.STATUS:
                        return incidents.OrderBy(x => x.IncidentStatus);
                    case IncidentFields.TYPE:
                        return incidents.OrderBy(x => x.WorkType);
                    case IncidentFields.VOLTAGELEVEL:
                        return incidents.OrderBy(x => x.VoltageLevel);

                }

            }
            else
            {
                switch (sortBy)
                {
                    case IncidentFields.ID:
                        return incidents.OrderByDescending(x => x.ID);
                    case IncidentFields.ATA:
                        return incidents.OrderByDescending(x => x.ATA);
                    case IncidentFields.CONFIRMED:
                        return incidents.OrderByDescending(x => x.Confirmed);
                    case IncidentFields.ETA:
                        return incidents.OrderByDescending(x => x.ETA);
                    case IncidentFields.ETR:
                        return incidents.OrderByDescending(x => x.ETR.Value.Hour)
                                         .ThenBy(x => x.ETR.Value.Minute);
                    case IncidentFields.INCIDENTDATETIME:
                        return incidents.OrderByDescending(x => x.IncidentDateTime);
                    case IncidentFields.PLANNEDWORK:
                        return incidents.OrderByDescending(x => x.WorkBeginDate);
                    case IncidentFields.PRIORITY:
                        return incidents.OrderByDescending(x => x.Priority);
                    case IncidentFields.STATUS:
                        return incidents.OrderByDescending(x => x.IncidentStatus);
                    case IncidentFields.TYPE:
                        return incidents.OrderByDescending(x => x.WorkType);
                    case IncidentFields.VOLTAGELEVEL:
                        return incidents.OrderByDescending(x => x.VoltageLevel);
            }

            }

            return incidents;
        }

        public IncidentStatisticsDto GetStatisticsForUser(int userId)
        {
            IQueryable<Incident> incidents = _dbContext.Incidents.Where(x => x.UserID == userId);

            IncidentStatisticsDto statistics = new IncidentStatisticsDto()
            {
                Total = incidents.Count(),
                Planned = incidents.Where(x => x.WorkType == WorkType.PLANNED).Count(),
                Unplanned = incidents.Where(x => x.WorkType == WorkType.UNPLANNED).Count(),
                Resolved = incidents.Where(x => x.IncidentStatus == IncidentStatus.RESOLVED).Count(),
                Unresolved = incidents.Where(x => x.IncidentStatus == IncidentStatus.UNRESOLVED).Count(),
            };

            return statistics;

        }
    }
}
