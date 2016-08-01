using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc
{
    public sealed class JsonContentData<T> : JsonData
    {
        T _data;

        public JsonContentData()
            : base(true, string.Empty)
        { }

        public JsonContentData(T data, bool success, string message)
            : base(success, message)
        {
            _data = data;
        }

        public T data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
