using SmartEnergy.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Contract.Interfaces
{
    public interface IDeviceUsageService : IGenericService<DeviceUsageDto>
    {
        void CopyIncidentDevicesToWorkRequest(int incidentID, int workRequestID);

        List<DeviceDto> GetSafetyDocumentDevices(int sfID);

        List<DeviceDto> GetWorkRequestDevices(int wrID);

        List<DeviceDto> GetIncidentDevices(int incID);

        void RemoveDeviceFromIncident(int incidentId, int deviceId);

        public void CopyIncidentDevicesToSafetyDocumentAsync(int workPlanId, int safetyDocumentId);
    }
}
