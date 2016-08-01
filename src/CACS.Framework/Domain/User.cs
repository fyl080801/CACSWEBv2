using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACS.Framework.Domain
{
    public class User : IdentityUser
    {
        /// <summary>
        /// 名
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [NotMapped]
        public virtual string PersonalName
        {
            get { return string.Format("{1} {0}", FirstName, LastName); }
        }
    }
}
