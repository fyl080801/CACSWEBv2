using CACS.Framework.Data;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Profiles;
using CACSLibrary.Profile;
using System.Web.Mvc;

namespace CACS.WebSite.Controllers
{
    public class HomeController : CACSController
    {
        IProfileManager _profileManager;
        IDataSettingsManager _dataSettingManager;

        public HomeController(
            IProfileManager profileManager,
            IDataSettingsManager dataSettingManager)
        {
            _profileManager = profileManager;
            _dataSettingManager = dataSettingManager;
        }

        public ActionResult Index()
        {
            bool installed = _dataSettingManager.LoadSettings().IsInstalled;
            if (!installed)
            {
                return RedirectToAction("Wizard", "Installer");
            }
            else
            {
                var settings = _profileManager.Get<GlobalSettings>();
                return Redirect(Url.Content(settings.IndexUrl));
            }
        }

        [AccountTicket]
        public ActionResult Default()
        {
            return View();
        }

        public ActionResult Error(HandleErrorInfo model)
        {
            return View(model);
        }
    }
}