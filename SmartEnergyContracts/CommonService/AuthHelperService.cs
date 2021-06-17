
using SmartEnergy.Contract.Interfaces;
using System.Linq;
using System.Security.Claims;

namespace SmartEnergy.Contract.CommonService
{
    public class AuthHelperService : IAuthHelperService
    {
        public AuthHelperService()
        {

        }

       
        public int GetUserIDFromPrincipal(ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        
    }
}
