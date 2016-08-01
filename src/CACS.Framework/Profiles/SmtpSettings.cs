using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Profiles
{
    public class SmtpSettings : BaseProfile
    {
        public string Address { get; set; }

        public string Server { get; set; }

        public int Port { get; set; }

        public bool IsCredential { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public override ProfileObject GetDefault()
        {
            return new SmtpSettings()
            {
                Port = 25
            };
        }
    }
}
