using CACS.Framework.Domain;
using CACS.Framework.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Identity
{
    public class ApplicationUserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public ApplicationUserStore(DbContext context) : base(context)
        {
        }
    }
}
