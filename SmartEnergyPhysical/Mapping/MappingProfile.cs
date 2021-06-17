using AutoMapper;
using SmartEnergy.Contract.DTO;
using SmartEnergy.Physical.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEnergy.Physical.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<LocationDto, Location>();
            CreateMap<Location, LocationDto>();

            CreateMap<DeviceDto, Device>()
                .ForMember(mem => mem.DeviceType, op => op.MapFrom(o => o.DeviceType));
            CreateMap<Device, DeviceDto>()
                .ForMember(mem => mem.DeviceType, op => op.MapFrom(o => o.DeviceType));

            CreateMap<DeviceUsage, DeviceUsageDto>();
            CreateMap<DeviceUsageDto, DeviceUsage>();

        }
    }
}
