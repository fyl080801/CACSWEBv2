using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc
{
    public class JsonListData<T> : JsonData
    {
        T[] _data;
        int _total;

        public JsonListData(bool success, T[] data)
            : base(success, string.Empty)
        {
            _data = data;
        }

        public JsonListData(bool success, T[] data, int total)
            : base(success, string.Empty)
        {
            _data = data;
            _total = total;
        }

        public JsonListData(bool success, T[] data, int total, string message)
            : base(success, message)
        {
            _data = data;
            _total = total;
        }

        public T[] data
        {
            get { return _data; }
        }

        public int total
        {
            get { return _total; }
        }
    }
}
