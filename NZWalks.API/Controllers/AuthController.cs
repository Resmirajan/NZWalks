using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenrepository tokenrepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenrepository tokenrepository)
        {
            this.userManager = userManager;
            this.tokenrepository = tokenrepository;
        }
        //POST  /api/Auth/Register

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult>Register([FromBody]RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName=registerRequestDto.Username,
                Email=registerRequestDto.Username
            };
          var identityResult=  await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if(identityResult.Succeeded)
            {
                //Add roles to this user
                if(registerRequestDto.Roles!=null&& registerRequestDto.Roles.Any())
                {
                  identityResult=  await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if(identityResult.Succeeded)
                    {
                        return Ok("User was registered ! Please Login");
                    }
                }
                
            }

            return BadRequest("Something went wrong");

        }

        //POST /api/Auth/Login
        [HttpPost]
        [Route("Login")]
         
        public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequestDto)
        {
           var user= await userManager.FindByEmailAsync(loginRequestDto.Username);
            if(user!=null)
            {
               var checkPasswordResult= await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if(checkPasswordResult)
                {
                    //get roles for this user
                    var roles =await userManager.GetRolesAsync(user);
                    if(roles!=null)
                    {
                        //create Token
                        var jwtToken = tokenrepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };
                        return Ok(response);
                    }
                    
                }

            }
            return BadRequest("Username or PassWord Incorrect");
        }


    }
}
