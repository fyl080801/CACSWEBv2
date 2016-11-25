using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc.Models
{
    public class RoleMemberModel : BaseEntityModel<int>
    {
        public string UserName { get; set; }

        public string PersonalName { get; set; }

        public bool IsMember { get; set; }

        public int RoleId { get; set; }
    }
}
