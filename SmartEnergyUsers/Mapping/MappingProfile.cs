using AutoMapper;
using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Enums;
using SmartEnergy.Users.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEnergy.Users.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CrewDto, Crew>();
            CreateMap<Crew, CrewDto>();

            CreateMap<UserDto, User>()
                .ForMember(mem => mem.UserStatus, op => op.MapFrom(o => o.UserStatus))
                .ForMember(mem => mem.UserType, op => op.MapFrom(o => o.UserType)); 
            CreateMap<User, UserDto>()
                .ForMember(mem => mem.UserStatus, op => op.MapFrom(o => o.UserStatus))
                .ForMember(mem => mem.UserType, op => op.MapFrom(o => o.UserType));

            CreateMap<Consumer, ConsumerDto>()
                .ForMember(mem => mem.AccountType, op => op.MapFrom(o => o.AccountType));

            CreateMap<ConsumerDto, Consumer>()
                 .ForMember(mem => mem.AccountType, op => op.MapFrom(o => o.AccountType));

        }
    }
}
