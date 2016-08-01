using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Interfaces;
using CACSLibrary;
using CACSLibrary.Data;
using CACSLibrary.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CACS.Services
{
    public class LogService : ILogService
    {
        readonly IRepository<EventLog> _logRepository;
        readonly IWebHelper _webHelper;
        readonly ApplicationUserManager _userManager;
        readonly HttpContextBase _httpContext;

        public LogService(
            IRepository<EventLog> logRepository,
            IWebHelper webHelper,
            HttpContextBase httpContext,
            ApplicationUserManager userManager)
        {
            _logRepository = logRepository;
            _webHelper = webHelper;
            _httpContext = httpContext;
            _userManager = userManager;
        }

        public void AddLog(EventLog log)
        {
            if (_httpContext.User == null) return;
            log.EventTime = DateTime.Now;
            log.UserId = _userManager.FindByName(_httpContext.User.Identity.Name).Id;
            log.IpAddress = _webHelper.GetCurrentIpAddress();
            _logRepository.Insert(log);
        }

        public EventLog GetEventById(int eventId)
        {
            return _logRepository.GetById(eventId);
        }

        public string[] LoadAllSourceId()
        {
            var query = _logRepository.Table.Select(m => m.SourceId).Distinct();
            return query.ToArray();
        }

        public IPagedList<EventLog> GetAllLog(
            string[] sourceIds,
            string personalName,
            int[] eventTypes,
            string eventName,
            DateTime? timeFrom,
            DateTime? timeTo,
            int pageIndex,
            int pageSize,
            IDictionary<string, bool> orders)
        {
            var query = _logRepository.Table;
            if (timeFrom.HasValue)
                query = query.Where(c => timeFrom.Value <= c.EventTime);
            if (timeTo.HasValue)
            {
                timeTo = timeTo.Value.AddDays(1.0);
                query = query.Where(c => timeTo.Value >= c.EventTime);
            }
            if (sourceIds != null && sourceIds.Length > 0)
                query = query.Where(c => sourceIds.Any(m => m.Equals(c.SourceId)));
            if (personalName != null && personalName.Length > 0)
                query = query.Where(c => c.User.PersonalName.Contains(personalName));
            if (eventTypes != null && eventTypes.Length > 0)
                query = query.Where(c => eventTypes.Any(m => m == (int)c.EventType));
            if (!String.IsNullOrWhiteSpace(eventName))
                query = query.Where(c => c.EventName.Contains(eventName));
            if (orders != null && orders.Count > 0)
            {
                foreach (var item in orders)
                {
                    if (item.Key == "Id")
                        query = item.Value ? query.OrderBy(m => m.Id) : query.OrderByDescending(m => m.Id);
                    else if (item.Key == "EventName")
                        query = item.Value ? query.OrderBy(m => m.EventName) : query.OrderByDescending(m => m.EventName);
                    else if (item.Key == "SourceName")
                        query = item.Value ? query.OrderBy(m => m.SourceName) : query.OrderByDescending(m => m.SourceName);
                    else if (item.Key == "EventTypeName")
                        query = item.Value ? query.OrderBy(m => m.EventType) : query.OrderByDescending(m => m.EventType);
                    else if (item.Key == "PersonalName")
                        query = item.Value ? query.OrderBy(m => m.User.PersonalName) : query.OrderByDescending(m => m.User.PersonalName);
                    else
                        query = QueryBuilder.DataSorting(query, item.Key, item.Value);
                }
            }
            else
            {
                query = query.OrderByDescending(m => m.EventTime);
            }
            var logs = new PagedList<EventLog>(query, pageIndex, pageSize);
            return logs ?? new PagedList<EventLog>(new List<EventLog>(), pageIndex, pageSize);
        }

        public IPagedList<EventLog> GetLogByKeyword(
            string keyWord,
            DateTime? fromDate,
            DateTime? toDate,
            int pageIndex,
            int pageSize,
            IDictionary<string, bool> orders)
        {
            var query = _logRepository.Table;
            if (!String.IsNullOrWhiteSpace(keyWord))
                query = query.Where(c => c.EventName.Contains(keyWord) || c.User.PersonalName.Contains(keyWord));

            if (fromDate.HasValue)
                query = query.Where(c => c.EventTime >= fromDate);
            if (toDate.HasValue)
                query = query.Where(c => c.EventTime <= toDate);
            if (orders != null && orders.Count > 0)
            {
                foreach (var item in orders)
                {
                    if (item.Key == "Id")
                        query = item.Value ? query.OrderBy(m => m.Id) : query.OrderByDescending(m => m.Id);
                    else if (item.Key == "EventName")
                        query = item.Value ? query.OrderBy(m => m.EventName) : query.OrderByDescending(m => m.EventName);
                    else if (item.Key == "SourceName")
                        query = item.Value ? query.OrderBy(m => m.SourceName) : query.OrderByDescending(m => m.SourceName);
                    else if (item.Key == "EventTypeName")
                        query = item.Value ? query.OrderBy(m => m.EventType) : query.OrderByDescending(m => m.EventType);
                    else if (item.Key == "PersonalName")
                        query = item.Value ? query.OrderBy(m => m.User.PersonalName) : query.OrderByDescending(m => m.User.PersonalName);
                    else
                        query = QueryBuilder.DataSorting(query, item.Key, item.Value);
                }
            }
            else
            {
                query = query.OrderByDescending(m => m.EventTime);
            }
            var logs = new PagedList<EventLog>(query, pageIndex, pageSize);
            return logs ?? new PagedList<EventLog>(new List<EventLog>(), pageIndex, pageSize);
        }
    }
}
