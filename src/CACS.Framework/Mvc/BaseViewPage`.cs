using CACS.Framework.Data;
using CACSLibrary.Infrastructure;
using CACSLibrary.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc
{
    public abstract class BaseViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        //IUserContext _userContext;
        IPluginFinder _pluginFinder;

        //public IUserContext UserContext
        //{
        //    get { return _userContext; }
        //}

        public IPluginFinder PluginFinder
        {
            get { return _pluginFinder; }
        }

        public bool IsInstall { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            bool installed = EngineContext.Current.Resolve<IDataSettingsManager>().LoadSettings().IsInstalled;
            if (installed)
            {
                InitInstalled();
            }
            else
            {
                InitUninstall();
            }
        }

        //public virtual string GetPluginPath(string pluginId)
        //{
        //    var pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();
        //    var plugin = pluginFinder.GetPluginDescriptorById(pluginId);
        //    if (plugin != null)
        //    {
        //        string path = plugin.PluginPath.Replace(Server.MapPath("/"), "~/").Replace("\\", "/");
        //        string pluginpath = Url.Content("~/Plugins" + path.Substring(path.LastIndexOf("\\")));
        //        return pluginpath;
        //    }
        //    return string.Empty;
        //}

        protected virtual void InitInstalled()
        {
            IsInstall = true;
            //_userContext = EngineContext.Current.Resolve<IUserContext>();
            _pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();
        }

        protected virtual void InitUninstall()
        {
            IsInstall = false;
        }
    }

    public abstract class BaseViewPage : BaseViewPage<dynamic>
    {
    }
}
