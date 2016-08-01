using CACSLibrary.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace CACS.WebSite.Models.Plugin
{
    public class PluginModel
    {
        public string PluginId { get; set; }

        public string PluginName { get; set; }

        public string Description { get; set; }

        public string DependentOn { get; set; }

        public string Version { get; set; }

        public string Tags { get; set; }

        public bool Installed { get; set; }

        public static PluginModel PreparePluginModel(PluginDescription pluginDescription, IEnumerable<PluginDescription> plugins)
        {
            var deps = pluginDescription.DependentOn;
            string depstr = "";
            foreach (var dep in deps)
            {
                var plugin = plugins.FirstOrDefault(m => m.PluginId == dep.PluginId);
                depstr += (plugin == null ? dep.PluginId : plugin.PluginName) + "(" + dep.Version.ToString() + "), ";
            }
            int nameIndex = pluginDescription.PluginType.Namespace.LastIndexOf(".");
            var ppath = HostingEnvironment.ApplicationPhysicalPath;
            var vpath = pluginDescription.PluginFile.DirectoryName.Replace(ppath, "");
            return new PluginModel()
            {
                DependentOn = depstr,
                Description = pluginDescription.Remark,
                PluginId = pluginDescription.PluginId,
                PluginName = !string.IsNullOrEmpty(pluginDescription.PluginName) ? pluginDescription.PluginName : pluginDescription.PluginId,
                Version = pluginDescription.Version.ToString(),
                Tags = string.Join(", ", pluginDescription.Tags.ToArray()),
                Installed = pluginDescription.Installed
                //Path = "/" + vpath.Replace(@"\", @"/"),
                //TypeName = pluginDescription.PluginType.Namespace.Substring(nameIndex + 1, pluginDescription.PluginType.Namespace.Length - nameIndex - 1)
            };
        }
    }
}