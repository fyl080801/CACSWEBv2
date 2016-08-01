using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Profiles
{
    public abstract class UserDatabaseProfile : ProfileObject
    {
        public UserDatabaseProfile()
            : base(new UserDatabaseProfileProvider())
        { }
    }
}
