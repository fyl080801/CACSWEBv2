using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CACS.Framework.Domain
{
    [Table("sys_Message")]
    public class Message : BaseEntity<int>
    {
        [Required(AllowEmptyStrings = false), MaxLength(50)]
        public virtual string Title { get; set; }

        public virtual DateTime SendTime { get; set; }

        public virtual DateTime? ReadTime { get; set; }

        public virtual string Content { get; set; }

        public virtual bool IsHtml { get; set; }

        public virtual MessageTypes MessageType { get; set; }

        public virtual int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        public virtual int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }
    }

    public enum MessageTypes
    {
        Information = 0,
        Exclamation = 1,
        Error = 2,
        Remind = 3
    }
}
