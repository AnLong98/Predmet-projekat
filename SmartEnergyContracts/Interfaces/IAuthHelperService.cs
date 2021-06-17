using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SmartEnergy.Contract.Interfaces
{
    public interface IAuthHelperService
    {
        int GetUserIDFromPrincipal(ClaimsPrincipal user);
    }
}
