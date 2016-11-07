using CACS.Framework.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACS.Framework.Domain
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
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
