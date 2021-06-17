using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Contract.Interfaces
{
    public interface IDeviceService : IGenericService<DeviceDto>
    {

        DeviceListDto GetDevicesPaged(DeviceField sortBy, SortingDirection direction, int page, int perPage);
        DeviceListDto GetSearchDevicesPaged(DeviceField sortBy, SortingDirection direction, int page, int perPage, DeviceFilter type, DeviceField field, string searchParam);

       
    }
}
