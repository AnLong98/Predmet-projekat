using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartEnergy.Contract.Interfaces
{
    public interface IIncidentService : IGenericService<IncidentDto>
    {
        Task RevertIncidentToInitialState(int incidentID);

        Task<LocationDto> GetIncidentLocationAsync(int incidentId);

        Task<IncidentDto> AddCrewToIncidentAsync(int incidentId, int crewId);

        IncidentDto RemoveCrewFromIncidet(int incidentId);

        List<IncidentDto> GetUnassignedIncidents();

        Task AddDeviceToIncidentAsync(int incidentId, int deviceId);

        void RemoveDeviceFromIncindet(int incidentId, int deviceId);
        Task<List<IncidentMapDisplayDto>> GetUnresolvedIncidentsForMapAsync();

        Task<List<CallDto>> GetIncidentCallsAsync(int incidentId);

        Task<int> GetNumberOfCallsAsync(int incidentId);

        Task<int> GetNumberOfAffectedConsumersAsync(int incidentId);

        Task<List<DeviceDto>> GetIncidentDevicesAsync(int incidentId);

        void SetIncidentPriority(int incidentId);

        Task<List<DeviceDto>> GetUnrelatedDevicesAsync(int incidentId);

        Task<CrewDto> GetIncidentCrewAsync(int incidentId);

        Task<CallDto> AddIncidentCallAsync(int incidentId, CallDto newCall);

        void AssignIncidetToUser(int incidentId, int userId);


        IncidentListDto GetIncidentsPaged(IncidentFields sortBy, SortingDirection direction, int page,
                               int perPage, IncidentFilter filter, OwnerFilter owner,
                               string searchParam, ClaimsPrincipal user);

         IncidentStatisticsDto GetStatisticsForUser(int userId);




    }
}
