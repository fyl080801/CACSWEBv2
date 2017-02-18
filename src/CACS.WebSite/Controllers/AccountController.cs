using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Profiles;
using CACS.WebSite.Models.Account;
using CACSLibrary;
using CACSLibrary.Profile;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CACS.WebSite.Controllers
{
    public class AccountController : CACSController
    {
        IProfileManager _profileManager;
        ApplicationSignInManager _signinManager;
        ApplicationRoleManager _roleManager;
        ApplicationUserManager _userManager;
        ApplicationUserStore _userStore;
        IAuthenticationManager _authenticationManager;
        HttpContextBase _httpContext;

        public AccountController(
            IProfileManager profileManager,
            HttpContextBase httpContext)
        {
            _profileManager = profileManager;
            _signinManager = httpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _roleManager = httpContext.GetOwinContext().Get<ApplicationRoleManager>();
            _userManager = httpContext.GetOwinContext().Get<ApplicationUserManager>();
            _userStore = httpContext.GetOwinContext().Get<ApplicationUserStore>();
            _authenticationManager = _signinManager.AuthenticationManager;
            _httpContext = httpContext;
        }

        [HttpPost, Logging("用户登录")]
        public ActionResult Login(LoginModel model)
        {
            var result = _signinManager.PasswordSignIn(model.Username, model.Password, model.Remember, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return Json(true);
                case SignInStatus.LockedOut:
                    throw new CACSException("用户已锁定");
                case SignInStatus.RequiresVerification:
                    throw new CACSException("用户需要验证");
                case SignInStatus.Failure:
                default:
                    throw new CACSException("无效的登录尝试");
            }
        }

        public ActionResult Logout()
        {
            LoginModel model = new LoginModel();
            var userCookie = _profileManager.Get<LoginCookie>();
            if (User.Identity.IsAuthenticated)
            {
                model.Username = User.Identity.Name;
                model.Remember = userCookie.Remember;
            }
            _authenticationManager.SignOut(_signinManager.AuthenticationType);
            return Json(model);
        }

        [AccountTicket, HttpPost, Logging("修改密码")]
        public ActionResult ChangePassword(PasswordChangeModel model)
        {
            var userId = _httpContext.User.Identity.GetUserId();
            var result = _userManager.ChangePassword(Convert.ToInt32(userId), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
                return Json(true);
            else
                return Error(string.Join(",", result.Errors));
        }

        [AccountTicket(AuthorizeId = "/User/Save"), HttpPost, Logging("设置密码")]
        public async Task<ActionResult> SetPassword(PasswordSetModel model)
        {
            var user = _userManager.FindById(model.Id);
            if (user == null)
                return Error("用户不存在");
            var passwordhash = _userManager.PasswordHasher.HashPassword(model.Password);
            await _userStore.SetPasswordHashAsync(user, passwordhash);
            return Json(true);
        }
    }
}