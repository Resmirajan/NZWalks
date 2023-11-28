using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenrepository
    {
         string CreateJWTToken(IdentityUser user,List<string>roles);
    }
}
