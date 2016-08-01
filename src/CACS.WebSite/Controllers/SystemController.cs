using CACS.Framework.Data;
using CACS.Framework.Identity;
using CACS.Framework.Interfaces;
using CACS.Framework.Mvc.Controllers;
using CACS.WebSite.Models.Account;
using CACS.WebSite.Models.Plugin;
using CACS.WebSite.Models.System;
using CACSLibrary.Plugin;
using CACSLibrary.Profile;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CACS.WebSite.Controllers
{
    public class SystemController : CACSController
    {
        readonly IProfileManager _profileManager;
        readonly IPluginFinder _pluginFinder;
        readonly ISystemService _systemService;
        readonly IDataSettingsManager _dataSetting;
        readonly IAccountService _accountService;
        readonly ApplicationSignInManager _signinManager;

        public SystemController(
            IProfileManager profileManager,
            IPluginFinder pluginFinder,
            ISystemService systemService,
            IDataSettingsManager dataSetting,
            IAccountService accountService,
            HttpContextBase httpContext)
        {
            _profileManager = profileManager;
            _pluginFinder = pluginFinder;
            _systemService = systemService;
            _dataSetting = dataSetting;
            _accountService = accountService;
            _signinManager = httpContext.GetOwinContext().Get<ApplicationSignInManager>();
        }

        [HttpGet]
        public async Task<ActionResult> Information()
        {
            SystemModel model = new SystemModel();

            var systemInfo = _systemService.GetSystemInformation();
            var plugins = _pluginFinder.GetPluginDescriptors();
            var pluginEnumerator = plugins.GetEnumerator();
            if (systemInfo != null)
            {
                model.SystemId = systemInfo.SystemId;
                model.Version = systemInfo.Version;
            }
            model.Installed = _dataSetting.LoadSettings().IsInstalled;
            while (pluginEnumerator.MoveNext())
            {
                if (pluginEnumerator.Current.Installed)
                    model.Plugins.Add(PluginModel.PreparePluginModel(pluginEnumerator.Current, plugins));
            }

            UserInfo user = new UserInfo();
            if (User.Identity.IsAuthenticated)
            {
                var appuser = await _signinManager.UserManager.FindByNameAsync(User.Identity.Name);
                user.Id = appuser.Id;
                user.Username = User.Identity.Name;
                user.Vaild = true;

                var auths = _accountService.GetUserAuthorizes(appuser.Id);
                foreach (var auth in auths)
                {
                    model.Permissions.Add(auth);
                }
            }
            else
            {
                user.Vaild = false;
            }
            model.Session = user;
            return Json(model);
        }
    }
}