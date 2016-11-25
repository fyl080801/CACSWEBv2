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
    public class ApplicationRoleStore : RoleStore<Role, int, UserRole>
    {
        public ApplicationRoleStore(DbContext context) : base(context)
        {
        }
    }
}
