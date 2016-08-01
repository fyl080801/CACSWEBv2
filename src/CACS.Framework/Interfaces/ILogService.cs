using CACS.Framework.Domain;
using CACSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Interfaces
{
    public interface ILogService
    {
        void AddLog(EventLog log);

        EventLog GetEventById(int eventId);

        string[] LoadAllSourceId();

        IPagedList<EventLog> GetAllLog(
            string[] sourceIds,
            string personalName,
            int[] eventTypes,
            string eventName,
            DateTime? timeFrom,
            DateTime? timeTo,
            int pageIndex,
            int pageSize,
            IDictionary<string, bool> orders);

        IPagedList<EventLog> GetLogByKeyword(
            string keyWord,
            DateTime? fromDate,
            DateTime? toDate,
            int pageIndex,
            int pageSize,
            IDictionary<string, bool> orders);
    }
}
