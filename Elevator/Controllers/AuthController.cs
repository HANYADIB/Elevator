using Elevator.IService;
using Elevator.Models.auth;
using Elevator.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elevator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly RoleManager<IdentityRole> roleManager;


        public AuthController(IAuthService authService , RoleManager<IdentityRole> roleManager)
        {
            _authService=authService;
            this.roleManager=roleManager;

        }


        private string ApplicationUserId
        {
            get
            {
                string UserId = User.FindFirstValue("uid");
                return UserId;
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterModel model)
        {
            BaseResponse Response = new BaseResponse();
            Response.Result=true;
            Response.Errors=new List<string>();

            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.RegisterAsync(model);


                return Ok(result);
            }
            catch(Exception ex)
            {
                Response.Result=false;
                string ErrorMSG = "Exception :" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                Response.Errors.Add(ErrorMSG);
                return BadRequest(Response);
            }

        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginRequestModel model)
        {
            BaseResponse Response = new BaseResponse();
            Response.Result=true;
            Response.Errors=new List<string>();

            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.GetTokenAsync(model);
               

                return Ok(result);
            }
            catch(Exception ex)
            {
                Response.Result=false;
                string ErrorMSG = "Exception :" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                Response.Errors.Add(ErrorMSG);
                return BadRequest(Response);
            }
        }

    }
}
