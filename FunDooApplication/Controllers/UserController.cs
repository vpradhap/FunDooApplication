using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FunDooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }
        [HttpPost("Register")]
        public IActionResult Registration(UserRegistrationModel userRegistrationModel)
        {
            var result=userBL.Register(userRegistrationModel);
            if (result != null)
            {
                return this.Ok(new {success=true,message="Registered Successfully",data=result});
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Registertion Failed"});
            }
        }
        [HttpPost("Login")]
        public IActionResult Login(UserLoginModel userLoginModel)
        {
            var result = userBL.Login(userLoginModel);
            if(result != null)
            {
                return this.Ok(new { success = true, message = "Login Success", data = result });
            }
            else
            {
                return this.NotFound(new { success = false, message = "Invalid Login details"});
            }
        }
        [HttpPost("Forget")]
        public IActionResult ForgetPassword(UserForgetModel userForgetModel)
        {
            var result = userBL.ForgetPassword(userForgetModel);
            if (result != null)
            {
                return this.Ok(new { success = true, message = "Token sent to your mail successfully" });
            }
            else
            {
                return this.NotFound(new { success = false, message = "EmailId not registered,try again" });
            }
        }
        [Authorize]
        [HttpPost("Reset")]
        public IActionResult ResetPassword(UserResetModel userResetModel)
        {
            userResetModel.Email = User.FindFirst(ClaimTypes.Email).Value;
            var result = userBL.ResetPassword(userResetModel);
            if (result != null)
            {
                return this.Ok(new { success = true, message = "Reset success" });
            }
            else
            {
                return this.NotFound(new { success = false, message = "Reset failed" });
            }
        }
    }
}
