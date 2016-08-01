using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Identity
{
    public class FakeIIdentity : IIdentity
    {
        string _name;

        public FakeIIdentity(string name)
        {
            _name = name;
        }

        public string AuthenticationType
        {
            get
            {
                return "";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return false;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
