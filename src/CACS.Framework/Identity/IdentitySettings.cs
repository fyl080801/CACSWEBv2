using CACS.Framework.Profiles;
using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Identity
{
    public class IdentitySettings : BaseProfile
    {
        public bool AllowOnlyAlphanumericUserNames { get; set; } = false;

        public bool RequireUniqueEmail { get; set; } = true;

        public int RequiredLength { get; set; } = 0;

        public bool RequireNonLetterOrDigit { get; set; } = false;

        public bool RequireDigit { get; set; } = false;

        public bool RequireLowercase { get; set; } = false;

        public bool RequireUppercase { get; set; } = false;

        public override ProfileObject GetDefault()
        {
            return new IdentitySettings();
        }
    }
}
