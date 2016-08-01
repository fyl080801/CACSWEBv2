using CACSLibrary.Profile;
using CACSLibrary.Web.Cookie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Profiles
{
    public class LoginCookie : CookieObject
    {
        string _cookieName = "CACS.USERMEMORY";

        public override string CookieName
        {
            get { return _cookieName; }
            set { _cookieName = value; }
        }

        [CookieProperty(IsCookieMark = true)]
        public string Username { get; set; }

        [CookieProperty(IsCookieMark = true)]
        public bool Remember { get; set; }

        public override ProfileObject GetDefault()
        {
            return new LoginCookie() { Remember = false };
        }
    }
}
