using CACSLibrary.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACS.Framework.Domain
{
    [Table("sys_Profile")]
    public class Profile : BaseEntity<int>
    {
        [Required(AllowEmptyStrings = false), MaxLength(50)]
        public virtual string Key { get; set; }

        [Required]
        public virtual string Value { get; set; }

        public virtual string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
