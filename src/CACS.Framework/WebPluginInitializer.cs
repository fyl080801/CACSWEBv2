using CACS.Framework;
using CACSLibrary.Component;
using CACSLibrary.Infrastructure;
using CACSLibrary.Plugin;
using CACSLibrary.Web.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(WebPluginInitializer), "InitializeWebPluginManager")]

namespace CACS.Framework
{
    public class WebPluginInitializer
    {
        static readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

        public static void InitializeWebPluginManager()
        {
            WebPluginManager pluginManager = new WebPluginManager();
            using (new WriteLocker(_locker))
            {
                pluginManager.Initialize();
                Singleton<IPluginManager>.Instance = pluginManager;
            }
        }
    }
}