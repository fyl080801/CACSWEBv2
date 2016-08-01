using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace CACS.Framework.Domain.Mapping
{
    public class EventLogMap : EntityTypeConfiguration<EventLog>
    {
        public EventLogMap()
        {
            this.HasKey(c => c.Id);
        }
    }
}
