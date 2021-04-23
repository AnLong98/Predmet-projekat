﻿using SmartEnergy.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Contract.Interfaces
{
    public interface IUserService : IGenericService<UserDto>
    {
        public List<UserDto> GetAllUnassignedCrewMembers();
        public UserDto ApproveUser(int userId);
        public UserDto DenyUser(int userId);
    }
}