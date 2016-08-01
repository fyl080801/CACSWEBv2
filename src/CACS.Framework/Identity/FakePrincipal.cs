using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Identity
{
    public class FakePrincipal : IPrincipal
    {
        IIdentity _identity;

        public FakePrincipal(IIdentity identity)
        {
            _identity = identity;
        }

        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }
}
