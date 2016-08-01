using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace CACS.Framework.Profiles
{
    public class SystemInformation : ProfileObject
    {
        public SystemInformation()
            : base(new XmlProfileProvider(HostingEnvironment.MapPath("~/App_Data")))
        {
            this.SystemId = Guid.NewGuid().ToString();
        }

        public string SystemId { get; set; }

        public string Version { get; set; }

        public DateTime? InstallTime { get; set; }

        public override ProfileObject GetDefault()
        {
            return new SystemInformation()
            {
                SystemId = Guid.NewGuid().ToString(),
                Version = "2.0"
            };
        }
    }
}
