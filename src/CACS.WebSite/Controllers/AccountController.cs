using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Profiles;
using CACS.WebSite.Models.Account;
using CACSLibrary;
using CACSLibrary.Profile;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace CACS.WebSite.Controllers
{
    public class AccountController : CACSController
    {
        IProfileManager _profileManager;
        ApplicationSignInManager _signinManager;
        ApplicationRoleManager _roleManager;
        IAuthenticationManager _authenticationManager;

        public AccountController(
            IProfileManager profileManager,
            HttpContextBase httpContext)
        {
            _profileManager = profileManager;
            _signinManager = httpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _roleManager = httpContext.GetOwinContext().Get<ApplicationRoleManager>();
            _authenticationManager = _signinManager.AuthenticationManager;
        }

        [HttpPost, Logging("用户登录")]
        public ActionResult Login(LoginModel model)
        {
            var result = _signinManager.PasswordSignIn(model.Username, model.Password, model.Remember, false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        HttpContext.User = new FakePrincipal(new FakeIIdentity(model.Username));
                        return Json(true);
                    }
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
    }
}