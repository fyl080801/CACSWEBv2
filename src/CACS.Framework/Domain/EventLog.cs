using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CACS.Framework.Domain
{
    [Table("sys_EventLog")]
    public class EventLog : BaseEntity<int>
    {
        [Required(AllowEmptyStrings = false), MaxLength(50)]
        public virtual string SourceId { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(50)]
        public virtual string SourceName { get; set; }

        public virtual DateTime EventTime { get; set; }

        public virtual int UserId { get; set; }

        [MaxLength(50)]
        public virtual string IpAddress { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(50)]
        public virtual string EventName { get; set; }

        public virtual string EventData { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual EventTypes EventType { get; set; }
    }

    public enum EventTypes
    {
        Information = 0,
        Create = 1,
        Delete = 2,
        Update = 3,
        Special = 4,
    }
}
