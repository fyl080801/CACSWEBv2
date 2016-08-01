using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace CACS.Framework.Domain.Mapping
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            this.HasKey(c => c.Id);
            this.HasRequired(e => e.Receiver)
                .WithMany()
                .HasForeignKey(e => e.ReceiverId)
                .WillCascadeOnDelete(false);
        }
    }
}
