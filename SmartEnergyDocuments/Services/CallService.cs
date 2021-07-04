using AutoMapper;
using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Interfaces;
using SmartEnergy.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SmartEnergy.Contract.CustomExceptions.Call;
using SmartEnergy.Contract.Enums;
using SmartEnergy.Contract.CustomExceptions.Incident;
using SmartEnergy.Documents.DomainModels;
using Dapr.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartEnergy.Documents.Services
{
    public class CallService : ICallService
    {

        private readonly DocumentsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly DaprClient _daprClient;

        public CallService(DocumentsDbContext dbContext, IMapper mapper, DaprClient daprClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _daprClient = daprClient;
        }

        public void Delete(int id)
        {
            Call call = _dbContext.Calls.FirstOrDefault(x => x.ID == id);

            if (call == null)
                throw new CallNotFoundExcpetion("Call not found!");

            _dbContext.Calls.Remove(call);
            _dbContext.SaveChanges();
        }

        public async Task< CallDto> GetAsync(int id)
        {
            return _mapper.Map<CallDto>(_dbContext.Calls.FirstOrDefault(x => x.ID == id));
        }

        public async System.Threading.Tasks.Task<List<CallDto>> GetAllAsync()
        {
            List<CallDto> calls = _mapper.Map<List<CallDto>>(_dbContext.Calls.ToList());
            foreach(CallDto call in calls)
            {
                try
                {
                    call.Location = await _daprClient.InvokeMethodAsync<LocationDto>(HttpMethod.Get, "smartenergyphysical", $"/api/locations/{call.LocationID}");
                    call.Consumer = await _daprClient.InvokeMethodAsync<ConsumerDto>(HttpMethod.Get, "smartenergyusers", $"/api/consumers/{call.ConsumerID}");
                }
                catch 
                {

                }
            }
            return calls;
                                                           
        }

        public async Task<CallDto> InsertAsync(CallDto entity)
        {
            if (entity.Hazard.Trim().Equals("") || entity.Hazard == null)
               throw new InvalidCallException("You have to enter call hazard!");

            if (!Enum.IsDefined(typeof(CallReason), entity.CallReason))
                throw new InvalidCallException("Undefined call reason!");


            /*if (_dbContext.Location.Any(x => x.ID == entity.LocationID) == false)
                throw new LocationNotFoundException($"Location with id = {entity.LocationID} does not exists!");*/


            if (entity.ConsumerID != 0 && entity.ConsumerID != null)
            {
                /*if (_dbContext.Consumers.Any(x => x.ID == entity.ConsumerID) == false)
                    throw new ConsumerNotFoundException($"Consumer with id = {entity.ConsumerID} does not exists!");*/
            }else
            {
                entity.ConsumerID = null;
            }


            Call newCall = _mapper.Map<Call>(entity);

            newCall.ID = 0;
            newCall.Incident = null;




            _dbContext.Calls.Add(newCall);
            _dbContext.SaveChanges();

            return _mapper.Map<CallDto>(newCall);



        }

        public async Task< CallDto> UpdateAsync(CallDto entity)
        {
            Call updatedCall = _mapper.Map<Call>(entity);
            Call oldCall = _dbContext.Calls.FirstOrDefault(x => x.ID.Equals(updatedCall.ID));


            //updatedCall.Location = null;
           
            if (oldCall == null)
                throw new CallNotFoundExcpetion($"Call with Id = {updatedCall.ID} does not exists!");

            if (updatedCall.Hazard.Trim().Equals("") || updatedCall.Hazard == null)
                throw new InvalidCallException("You have to enter hazard!");

            if (!Enum.IsDefined(typeof(CallReason), entity.CallReason))
                throw new InvalidCallException("Undefined call reason!");

 

            /*if (_dbContext.Location.Where(x => x.ID.Equals(updatedCall.LocationID)) == null)
                throw new LocationNotFoundException($"Location with id = {updatedCall.LocationID} does not exists!");*/


            if (_dbContext.Incidents.Where(x => x.ID.Equals(updatedCall.IncidentID)) == null)
                throw new IncidentNotFoundException($"Incident with id = {updatedCall.IncidentID} does not exists!");


            oldCall.UpdateCall(updatedCall);
            _dbContext.SaveChanges();

            return _mapper.Map<CallDto>(oldCall);
        }

        public async  Task RemoveCallsFromIncident(int incidentID)
        {
            List<Call> incidentCalls = _dbContext.Calls.Where(x => x.IncidentID == incidentID).ToList();

            foreach(Call c in incidentCalls)
            {
                c.IncidentID = null;
            }

            _dbContext.SaveChanges();
            return;
        }
    }
}
