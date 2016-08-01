using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Mvc.Filters
{
    public class LogFieldAttribute : Attribute
    {
        int _index;
        string _fieldName;
        string _fieldValue;

        public int Index
        {
            get { return _index; }
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        public string FieldValue
        {
            get { return _fieldValue; }
        }

        public LogFieldAttribute()
            : this(1)
        {
        }

        public LogFieldAttribute(int index)
            : base()
        {
            _index = index;
        }

        internal void GetPropertyDescription(PropertyInfo propertyInfo, object sender)
        {
            DisplayNameAttribute display = Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
            if (display != null)
            {
                this._fieldName = display.DisplayName;
            }
            else
            {
                this._fieldName = propertyInfo.Name;
            }
            object value = propertyInfo.GetValue(sender, null);
            this._fieldValue = value == null ? "" : value.ToString();
        }
    }
}
