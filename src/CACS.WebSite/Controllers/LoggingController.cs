using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Interfaces;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Mvc.Models;
using CACS.WebSite.Models.Account;
using CACSLibrary;
using CACSLibrary.Data;
using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CACS.WebSite.Controllers
{
    public class LoggingController : CACSController
    {
        IProfileManager _profileManager;
        ApplicationUserManager _userManager;
        ILogService _logService;
        IRepository<EventLog> _logRepository;

        public LoggingController(
            IProfileManager profileManager,
            ApplicationUserManager userManager,
            ILogService logService,
            IRepository<EventLog> logRepository)
        {
            _profileManager = profileManager;
            _userManager = userManager;
            _logService = logService;
            _logRepository = logRepository;
        }

        [AccountTicket(AuthorizeId = "/Logging/List")]
        public ActionResult Details(int id)
        {
            var result = _logService.GetEventById(id);
            return Json(EventLogModel.Prepare(result));
        }

        [AccountTicket(AuthorizeName = "查看日志", Group = "系统管理")]
        public ActionResult List(ListModel model)
        {
            var query = _logRepository.Table;
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.EventName.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderByDescending(e => e.EventTime);
            else
                model.Sort.ForEach(sort =>
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false));

            var result = new PagedList<EventLog>(query, model.Page - 1, model.Limit)
                ?? new PagedList<EventLog>(new List<EventLog>(), model.Page - 1, model.Limit);

            return JsonList(result.Select(e => (EventLog)e.Clone()).ToArray(), result.TotalCount);
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