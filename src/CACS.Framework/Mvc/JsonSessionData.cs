using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc
{
    public sealed class JsonSessionData : JsonData
    {
        bool _session = true;
        string _username;

        public JsonSessionData(bool success, bool session, string message)
            : base(success, message)
        {
            _session = session;
        }

        public bool session
        {
            get { return _session; }
        }

        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
    }
}
