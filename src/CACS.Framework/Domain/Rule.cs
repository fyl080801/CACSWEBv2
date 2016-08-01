using CACSLibrary.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACS.Framework.Domain
{
    [Table("sys_Rule")]
    public class Rule : BaseEntity<int>
    {
        [Required]
        public virtual string AuthorizeId { get; set; }

        public virtual string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
