using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.WebSite.Models.Plugin;
using CACSLibrary;
using CACSLibrary.Plugin;
using CACSLibrary.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CACS.WebSite.Controllers
{
    [Description("插件管理")]
    public class PluginController : CACSController
    {
        IPluginFinder _pluginFinder;
        IWebHelper _webHelper;

        public PluginController(
            IPluginFinder pluginFinder,
            IWebHelper webHelper)
        {
            _pluginFinder = pluginFinder;
            _webHelper = webHelper;
        }

        [AccountTicket(AuthorizeName = "插件管理", Group = "系统管理")]
        public ActionResult List(string keyword)
        {
            var plugins = _pluginFinder.GetPluginDescriptors();
            var pluginEnumerator = plugins.GetEnumerator();
            var pluginInfos = new List<PluginModel>();
            while (pluginEnumerator.MoveNext())
            {
                pluginInfos.Add(PluginModel.PreparePluginModel(pluginEnumerator.Current, plugins));
            }
            var query = pluginInfos.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(e => e.PluginName.Contains(keyword) || e.PluginId.Contains(keyword));
            return JsonList(query.ToArray());
        }

        public ActionResult Upload()
        {
            if (this.Request.Files.Count <= 0)
                throw new CACSException("未选择文件");
            var defineFile = this.Request.Files[0];
            Guid filename = Guid.NewGuid();
            var path = Server.MapPath("~/PluginTemp/");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var filepath = path + filename.ToString() + defineFile.FileName.Substring(defineFile.FileName.LastIndexOf("."));
            defineFile.SaveAs(filepath);
            return Json("");
        }

        [AccountTicket(AuthorizeId = "/Plugin/List")]
        public ActionResult Install(string id)
        {
            var plugins = _pluginFinder.GetPluginDescriptors();
            var plugin = plugins.FirstOrDefault(e => e.PluginId == id);
            if (plugin.Installed)
                throw new CACSException("插件已安装");
            plugin.Instance().Install();
            _webHelper.RestartAppDomain();
            return Json("");
        }

        [AccountTicket(AuthorizeId = "/Plugin/List")]
        public ActionResult Uninstall(string id)
        {
            var plugins = _pluginFinder.GetPluginDescriptors();
            var plugin = plugins.FirstOrDefault(e => e.PluginId == id);
            if (!plugin.Installed)
                throw new CACSException("插件未安装");
            plugin.Instance().Uninstall();
            _webHelper.RestartAppDomain();
            return Json("");
        }
    }
}