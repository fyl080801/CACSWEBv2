using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc.Models
{
    public class RoleAuthorizeModel : AuthorizeModel
    {
        public RoleAuthorizeModel()
        {

        }

        public RoleAuthorizeModel(AuthorizeModel model)
        {
            Group = model.Group;
            Id = model.Id;
            AuthorizeName = model.AuthorizeName;
        }

        public string RoleId { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
