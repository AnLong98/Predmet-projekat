using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartEnergy.Contract.Interfaces
{
    public interface IUserService : IGenericService<UserDto>
    {
        public List<UserDto> GetAllUnassignedCrewMembers();

        public Task<UsersListDto> GetUsersPagedAsync(UserField sortBy, SortingDirection direction, int page,
                                          int perPage, UserStatusFilter status, UserTypeFilter type,
                                          string searchParam);
        public UserDto ApproveUser(int userId);
        public UserDto DenyUser(int userId);

        public string Login(LoginDto userInfo, out UserDto user);
        public Task<LoginResponseDto> LoginExternal(ExternalLoginDto userInfo);
    }
}
