using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartEnergy.Contract.Interfaces
{
    public interface ISafetyDocumentService : IGenericService<SafetyDocumentDto>
    {

        Task<CrewDto> GetCrewForSafetyDocumentAsync(int safetyDocumentId);

        ChecklistDto UpdateSafetyDocumentChecklist(ChecklistDto checklistDto);

        Task<List<DeviceDto>> GetSafetyDocumentDevicesAsync(int safetyDocumentId);

        Task<List<SafetyDocumentDto>> GetAllMineSafetyDocumentsAsync(OwnerFilter owner, ClaimsPrincipal user);
     



    }
}
