using CACS.Framework.Profiles;
using CACS.WebSite.Models.Account;
using CACS.WebSite.Models.Plugin;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;
using System.Collections.Generic;

namespace CACS.WebSite.Models.System
{
    public class SystemModel
    {
        public string Title
        {
            get { return EngineContext.Current.Resolve<IProfileManager>().Get<GlobalSettings>().SiteTitle; }
        }

        public string SystemId { get; set; }

        public string Version { get; set; }

        public bool Installed { get; set; }

        public UserInfo Session { get; set; }

        public ICollection<string> Permissions { get; private set; } = new HashSet<string>();

        public ICollection<PluginModel> Plugins { get; private set; } = new HashSet<PluginModel>();
    }
}