using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc
{
    public class JsonData
    {
        bool _success = true;
        string _message;

        public JsonData(bool success, string message)
        {
            _success = success;
            _message = message;
        }

        public bool success
        {
            get { return _success; }
        }

        public string message
        {
            get { return _message; }
        }
    }
}
