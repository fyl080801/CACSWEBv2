using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc.Models
{
    public class IdentityAuthorizeModel : AuthorizeModel
    {
        public string IdentityId { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
