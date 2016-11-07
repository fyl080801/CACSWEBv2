using CACS.Framework.Domain;
using CACSLibrary;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CACS.Framework.Identity
{
    public class ApplicationRoleManager : RoleManager<Role, int>
    {
        public const string Administrators = "Administrators";

        public ApplicationRoleManager(IRoleStore<Role, int> store)
            : base(store)
        { }

        public override Task<IdentityResult> DeleteAsync(Role role)
        {
            if (role.Name == Administrators)
                throw new CACSException("管理员角色不能删除");
            return base.DeleteAsync(role);
        }
    }
}
