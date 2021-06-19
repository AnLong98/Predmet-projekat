using AutoMapper;
using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SmartEnergy.Contract.CustomExceptions.DeviceUsage;
using SmartEnergy.Physical.Infrastructure;
using SmartEnergy.Physical.DomainModels;
using Dapr.Client;
using System.Net.Http;
using SmartEnergy.Contract.CustomExceptions.SafetyDocument;
using SmartEnergy.Contract.CustomExceptions.WorkRequest;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartEnergy.Contract.CustomExceptions.Incident;

namespace SmartEnergy.Physical.Service
{
    public class DeviceUsageService : IDeviceUsageService
    {

        private readonly PhysicalDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly DaprClient _daprClient;

        public DeviceUsageService(PhysicalDbContext dbContext, IMapper mapper, DaprClient daprClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _daprClient = daprClient;
        }

        public async void CopyIncidentDevicesToSafetyDocumentAsync(int workPlanId, int safetyDocumentId)
        {
            try
            {
                SafetyDocumentDto sf = await _daprClient.InvokeMethodAsync<SafetyDocumentDto>(HttpMethod.Get, "smartenergydocuments", $"/api/safety-documents/{safetyDocumentId}");
            
            }catch
            {
                throw new SafetyDocumentNotFoundException($"Safety document with ID {safetyDocumentId} does not exist");
            }
                

            List<DeviceUsage> usages = _dbContext.DeviceUsages.Where(x => x.WorkPlanID == workPlanId).ToList();

         

            foreach (DeviceUsage deviceUsage in usages)
            {
                deviceUsage.SafetyDocumentID = safetyDocumentId;
            }

            _dbContext.SaveChanges();
        }

        public async void CopyIncidentDevicesToWorkRequest(int incidentID, int workRequestID)
        {
            try
            {
                WorkRequestDto wr = await _daprClient.InvokeMethodAsync<WorkRequestDto>(HttpMethod.Get, "smartenergydocuments", $"/api/work-requests/{workRequestID}");

            }catch
            {
                throw new WorkRequestNotFound($"Work request with ID {workRequestID} does not exist");
            }
                
            List<DeviceUsage> usages = _dbContext.DeviceUsages.Where(x => x.IncidentID == incidentID).ToList();

            foreach(DeviceUsage deviceUsage in usages)
            {
                deviceUsage.WorkRequestID = workRequestID;
            }

            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            DeviceUsage deviceUsage = _dbContext.DeviceUsages.FirstOrDefault(x => x.ID.Equals(id));

            if (deviceUsage == null)
                throw new DeviceUsageNotFoundException($"DeviceUsage with Id = {id} does not exists!");

            _dbContext.DeviceUsages.Remove(deviceUsage);
            _dbContext.SaveChanges();
        }

        public async Task<DeviceUsageDto> GetAsync(int id)
        {
            return _mapper.Map<DeviceUsageDto>(_dbContext.DeviceUsages.FirstOrDefault(x => x.ID == id));  // nisam vracao objekte incidenta itd
        }

        public async Task<List<DeviceUsageDto>> GetAllAsync()
        {
            return _mapper.Map<List<DeviceUsageDto>>(_dbContext.DeviceUsages.ToList());
        }

        public async Task<DeviceUsageDto> InsertAsync(DeviceUsageDto entity)
        {

            ValidateDeviceUsageAsync(entity);

            DeviceUsage newDeviceUsage = _mapper.Map<DeviceUsage>(entity);

            newDeviceUsage.ID = 0;
            newDeviceUsage.WorkPlanID = null;
            newDeviceUsage.WorkRequestID = null;
            newDeviceUsage.SafetyDocumentID = null;


            _dbContext.DeviceUsages.Add(newDeviceUsage);
            _dbContext.SaveChanges();

            return _mapper.Map<DeviceUsageDto>(newDeviceUsage);

        }

        public async Task<DeviceUsageDto> UpdateAsync(DeviceUsageDto entity)
        {

            ValidateDeviceUsageAsync(entity);

            DeviceUsage updatedDeviceUsage = _mapper.Map<DeviceUsage>(entity);
            DeviceUsage oldDeviceUsage = _dbContext.DeviceUsages.FirstOrDefault(x => x.ID.Equals(updatedDeviceUsage.ID));



            if (oldDeviceUsage == null)
                throw new DeviceUsageNotFoundException($"Device usage with Id = {updatedDeviceUsage.ID} does not exists!");

           /* if (entity.WorkRequestID != null)
            {
                try
                {
                    WorkRequestDto wr = await _daprClient.InvokeMethodAsync<WorkRequestDto>(HttpMethod.Get, "smartenergydocuments", $"/api/work-requests/{entity.WorkRequestID}");

                }
                catch
                {
                    throw new WorkRequestNotFound($"Work request with ID {entity.WorkRequestID} does not exist");
                }
            }


            if (entity.WorkPlanID != null)
            {
                if (_dbContext.WorkPlans.Any(x => x.ID == entity.WorkPlanID) == false)
                    throw new WorkPlanNotFoundException($"Work plan with id = {entity.WorkRequestID} does not exists!");
            }

            if (entity.SafetyDocumentID != null)
            {
                if (_dbContext.SafetyDocuments.Any(x => x.ID == entity.SafetyDocumentID) == false)
                    throw new SafetyDocumentNotFoundException($"Safety document with id = {entity.SafetyDocumentID} does not exists!");
            }
            */



            oldDeviceUsage.UpdateDeviceUsage(updatedDeviceUsage);
            _dbContext.SaveChanges();

            return _mapper.Map<DeviceUsageDto>(oldDeviceUsage);
        }

        private async void ValidateDeviceUsageAsync(DeviceUsageDto entity)
        {

            if (entity.IncidentID == null)
                throw new InvalidDeviceUsageException("Incident id can not be null!");

            if(entity.IncidentID != null)
            {
                try
                {
                    IncidentDto inc = await _daprClient.InvokeMethodAsync<IncidentDto>(HttpMethod.Get, "smartenergydocuments", $"/api/incidents/{entity.IncidentID}");

                }
                catch
                {
                    throw new IncidentNotFoundException($"Incident with ID {entity.IncidentID} does not exist");
                }

            }

           

            if (_dbContext.Devices.Any(x => x.ID == entity.DeviceID) == false)
                throw new DeviceUsageNotFoundException($"Device with id = {entity.DeviceID} does not exists!");


        }

        public List<DeviceDto> GetSafetyDocumentDevices(int sfID)
        {
            List<DeviceUsage> devUsages = _dbContext.DeviceUsages.Include(x => x.Device)
                                                                 .ThenInclude(x => x.Location)
                                                                 .Where(x => x.SafetyDocumentID == sfID).ToList();
            List<DeviceDto> devices = new List<DeviceDto>();

            foreach(var devUsage in devUsages)
            {
                devices.Add(_mapper.Map<DeviceDto>(devUsage.Device));
            }

            return devices;

        }

        public List<DeviceDto> GetWorkRequestDevices(int wrID)
        {
            List<DeviceUsage> devUsages = _dbContext.DeviceUsages.Include(x => x.Device)
                                                                 .ThenInclude(x => x.Location)
                                                                 .Where(x => x.WorkRequestID == wrID)
                                                                 .ToList();
            List<DeviceDto> devices = new List<DeviceDto>();

            foreach (var devUsage in devUsages)
            {
                devices.Add(_mapper.Map<DeviceDto>(devUsage.Device));
            }

            return devices;
        }

        public List<DeviceDto> GetIncidentDevices(int incID)
        {
            List<DeviceUsage> devUsages = _dbContext.DeviceUsages.Include(x => x.Device)
                                                                 .ThenInclude(x => x.Location)
                                                                 .Where(x => x.IncidentID == incID)
                                                                 .ToList();
            List<DeviceDto> devices = new List<DeviceDto>();

            foreach (var devUsage in devUsages)
            {
                devices.Add(_mapper.Map<DeviceDto>(devUsage.Device));
            }

            return devices;
        }

        public void RemoveDeviceFromIncident(int incidentId, int deviceId)
        {
            DeviceUsage deviceUsage = _dbContext.DeviceUsages.FirstOrDefault(x => x.IncidentID == incidentId && x.DeviceID == deviceId);

            if (deviceUsage == null)
                throw new DeviceUsageNotFoundException("Device could not be removed");
            _dbContext.DeviceUsages.Remove(deviceUsage);
            _dbContext.SaveChanges();
        }
    }
}
