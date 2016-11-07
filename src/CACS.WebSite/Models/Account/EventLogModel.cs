using CACS.Framework.Domain;
using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CACS.WebSite.Models.Account
{
    public class EventLogModel : BaseEntityModel<int>
    {
        public string SourceId { get; set; }

        public string SourceName { get; set; }

        public DateTime EventTime { get; set; }

        public string PersonalName { get; set; }

        public int UserId { get; set; }

        public string IpAddress { get; set; }

        public string EventName { get; set; }

        public string EventData { get; set; }

        public EventTypes EventType { get; set; }

        public static EventLogModel Prepare(EventLog domain)
        {
            return new EventLogModel()
            {
                Id = domain.Id,
                SourceId = domain.SourceId,
                SourceName = domain.SourceName,
                EventTime = domain.EventTime,
                UserId = domain.UserId,
                IpAddress = domain.IpAddress,
                EventName = domain.EventName,
                EventData = domain.EventData,
                EventType = domain.EventType,
                PersonalName = domain.User.PersonalName
            };
        }

        public EventLog ToDomain()
        {
            return new EventLog()
            {
                Id = this.Id,
                SourceId = this.SourceId,
                SourceName = this.SourceName,
                EventTime = this.EventTime,
                UserId = this.UserId,
                IpAddress = this.IpAddress,
                EventName = this.EventName,
                EventData = this.EventData,
                EventType = this.EventType,
            };
        }
    }
}