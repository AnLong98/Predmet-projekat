using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartEnergy.Contract.Interfaces
{
    public interface IStateChangeService
    {
        #region Work Request
        public WorkRequestDto ApproveWorkRequest(int workRequestId, ClaimsPrincipal user);
        public WorkRequestDto CancelWorkRequest(int workRequestId, ClaimsPrincipal user);
        public WorkRequestDto DenyWorkRequest(int workRequestId, ClaimsPrincipal user);
        public  Task<List<StateChangeHistoryDto>> GetWorkRequestStateHistoryAsync(int workRequestId);
        #endregion

        #region Safety document
        public SafetyDocumentDto ApproveSafetyDocument(int safetyDocId, ClaimsPrincipal user);
        public SafetyDocumentDto CancelSafetyDocument(int safetyDocId, ClaimsPrincipal user);
        public SafetyDocumentDto DenySafetyDocument(int safetyDocId, ClaimsPrincipal user);
        public Task<List<StateChangeHistoryDto>> GetSafetyDocumentStateHistoryAsync(int safetyDocId);
        #endregion
    }
}
