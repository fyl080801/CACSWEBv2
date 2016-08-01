using CACS.Framework.Profiles;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.ActionResults
{
    public class UnValidated : CACSResult
    {
        protected override ActionResult GetAjaxResult(ControllerContext context)
        {
            var profileManager = EngineContext.Current.Resolve<IProfileManager>();
            var lastLogin = profileManager.Get<LoginCookie>();
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new JsonSessionData(false, false, "登录已过期")
                {
                    username = lastLogin.Username
                }
            };
        }

        protected override ActionResult GetRequestResult(ControllerContext context)
        {
            var profileManager = EngineContext.Current.Resolve<IProfileManager>();
            var profile = profileManager.Get<GlobalSettings>();
            return new RedirectResult(profile.LoginUrl);
        }
    }
}
