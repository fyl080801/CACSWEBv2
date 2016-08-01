using CACS.Framework.Data;
using CACS.Framework.Interfaces;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Profiles;
using CACS.WebSite.Models.Installer;
using CACSLibrary;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using CACSLibrary.Plugin;
using CACSLibrary.Profile;
using CACSLibrary.Web;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Web.Mvc;

namespace CACS.WebSite.Controllers
{
    public class InstallerController : CACSController
    {
        IDataSettingsManager _dataSettingManager;
        BaseDataProviderManager _providerManager;
        IProfileManager _profileManager;
        IPluginManager _pluginManager;
        IPluginFinder _pluginFinder;
        IWebHelper _webHelper;

        public InstallerController(
            IDataSettingsManager dataSettingManager,
            BaseDataProviderManager providerManager,
            IProfileManager profileManager,
            IPluginManager pluginManager,
            IPluginFinder pluginFinder,
            IWebHelper webHelper)
        {
            _dataSettingManager = dataSettingManager;
            _providerManager = providerManager;
            _profileManager = profileManager;
            _pluginManager = pluginManager;
            _pluginFinder = pluginFinder;
            _webHelper = webHelper;
        }

        public ActionResult Wizard()
        {
            bool installed = _dataSettingManager.LoadSettings().IsInstalled;
            if (installed)
            {
                return RedirectToAction("Index", "Home");
            }
            var systemInformation = _profileManager.Get<SystemInformation>();
            _profileManager.Save(systemInformation.GetDefault());
            var model = _profileManager.Get<InstallerModel>();
            return View(model);
        }

        public ActionResult Datasources()
        {
            IList<string> _databaseInstances = new List<string>();
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            var sources = instance.GetDataSources();
            foreach (DataRow row in sources.Rows)
            {
                string serverName = row[0].ToString();
                string instanceName = row[1].ToString();
                _databaseInstances.Add(serverName + (string.IsNullOrEmpty(instanceName) ? "" : "\\" + instanceName));
            }
            return Json(_databaseInstances.Select(c => new { item = c }).ToArray());
        }

        [HttpPost]
        public ActionResult Install(InstallerModel model)
        {
            try
            {
                bool installed = _dataSettingManager.LoadSettings().IsInstalled;

                if (installed) throw new CACSException("系统已经安装");

                var setting = _dataSettingManager.LoadSettings();
                setting.ConnectionString = model.DatabaseConnectionString;
                setting.DataProvider = "sqlserver";
                setting.EntityMapAssmbly.Add("CACS.Framework");
                _dataSettingManager.SaveSettings(setting);

                _pluginManager.MakeAllPluginUninstalled();

                //
                var systemVersion = _profileManager.Get<SystemInformation>().Version;
                var plugins = _pluginFinder.GetOrderedPlugins();
                var orderPlugins = plugins.Where(c => c.SupportedVersion <= Version.Parse(systemVersion)).ToArray();
                foreach (var plugin in orderPlugins)
                {
                    plugin.Instance().Install();
                }

                _providerManager.LoadDataProvider().InitDatabase();

                EngineContext.Current.Resolve<IInstallation>().InstallData(
                    model.AdminPassword,
                    model.AdminEmail,
                    model.AdminEmail);

                _webHelper.RestartAppDomain();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _dataSettingManager.ClearSettings();
                throw new CACSException(ex.Message);
            }
        }
    }
}