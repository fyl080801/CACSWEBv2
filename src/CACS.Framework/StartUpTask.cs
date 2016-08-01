using CACS.Framework.Interfaces;
using CACS.Framework.Profiles;
using CACSLibrary;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework
{
    public class StartUpTask : IStartupTask
    {
        public void Execute()
        {
            var settings = EngineContext.Current.Resolve<IProfileManager>().Get<DatabaseInfo>();
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString)) return;
            //
            var provider = EngineContext.Current.Resolve<IEfDataProvider>();
            if (provider == null) throw new CACSException("没有注册 IEfDataProvider");
            provider.SetDatabaseInitializer();
        }

        public EngineLevels Level
        {
            get { return EngineLevels.Priority; }
        }
    }
}
