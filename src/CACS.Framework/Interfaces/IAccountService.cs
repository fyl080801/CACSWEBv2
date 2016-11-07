using CACS.Framework.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CACS.Framework.Interfaces
{
    public interface IAccountService
    {
        ICollection<AuthorizeModel> GetAuthorizes();

        ICollection<RoleAuthorizeModel> GetRoleAuthorizes(int id);

        ICollection<string> GetUserAuthorizes(int id);

        void SetRoleAuthorizes(AuthorizeModel[] authorizes, int roleId);
    }
}
