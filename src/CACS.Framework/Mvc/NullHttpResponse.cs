using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CACS.Framework.Mvc
{
    public class NullHttpResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection _cookies;
        private readonly StringBuilder _outputString = new StringBuilder();

        public NullHttpResponse()
        {
            this._cookies = new HttpCookieCollection();
        }

        public string ResponseOutput
        {
            get { return _outputString.ToString(); }
        }

        public override int StatusCode { get; set; }

        public override string RedirectLocation { get; set; }

        public override void Write(string s)
        {
            _outputString.Append(s);
        }

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }
    }
}
