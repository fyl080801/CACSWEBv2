using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc.Models
{
    public class AuthorizeModel : BaseEntityModel<string>
    {
        public string Group { get; set; }

        public string AuthorizeName { get; set; }
    }
}
