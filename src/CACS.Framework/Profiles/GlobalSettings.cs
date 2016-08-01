using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Profiles
{
    public class GlobalSettings : BaseProfile
    {
        public int LoginTimeout { get; set; }

        public int RememberDays { get; set; }

        public string IndexUrl { get; set; }

        public string LoginUrl { get; set; }

        public string SiteTitle { get; set; }

        public override ProfileObject GetDefault()
        {
            return new GlobalSettings
            {
                LoginTimeout = 15,
                RememberDays = 14,
                IndexUrl = "/Home/Default",
                LoginUrl = "/Account/Login",
                SiteTitle = "管理系统"
            };
        }
    }
}
