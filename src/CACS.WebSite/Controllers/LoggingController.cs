using CACS.Framework.Identity;
using CACS.Framework.Interfaces;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.WebSite.Models.Account;
using CACSLibrary.Profile;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CACS.WebSite.Controllers
{
    public class LoggingController : CACSController
    {
        IProfileManager _profileManager;
        ApplicationUserManager _userManager;
        ILogService _logService;

        public LoggingController(
            IProfileManager profileManager,
            //HttpContextBase httpContext,
            ApplicationUserManager userManager,
            ILogService logService)
        {
            _profileManager = profileManager;
            _userManager = userManager;//httpContext.GetOwinContext().Get<ApplicationUserManager>();
            _logService = logService;
        }

        public ActionResult Details(int id)
        {
            var result = _logService.GetEventById(id);
            return Json(EventLogModel.Prepare(result));
        }

        [AccountTicket]
        public JsonResult LoadLogs(LogListModel model, int limit, int page, string sort, string dir)
        {
            IDictionary<string, bool> dic = new Dictionary<string, bool>();
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(dir))
            {
                dic.Add(sort, dir == "ASC" ? true : false);
            }

            var logs = _logService.GetLogByKeyword(
                model.SearchKeyword,
                model.FromTime,
                model.ToTime,
                page - 1,
                limit,
                dic);
            return JsonList(
                logs.Select(EventLogModel.Prepare).ToArray(),
                logs.TotalCount);
        }
    }
}