using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace CACS.Framework.Profiles
{
    public class BaseProfile : ProfileObject
    {
        public BaseProfile()
            : base(new XmlProfileProvider(HostingEnvironment.MapPath("~/Profiles")))
        {

        }

        public override ProfileObject GetDefault()
        {
            return this;
        }
    }
}
