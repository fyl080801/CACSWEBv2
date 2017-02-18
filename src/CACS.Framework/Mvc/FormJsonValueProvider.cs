using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CACS.Framework.Mvc
{
    public class FormJsonValueProvider : IValueProvider
    {
        HttpContextBase _httpContext;

        public FormJsonValueProvider(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public bool ContainsPrefix(string prefix)
        {
            return _httpContext.Request.ContentType.Contains("application/x-www-form-urlencoded")
                && _httpContext.Request.Form.Keys.Count == 1
                && _httpContext.Request.Form.Keys[0] == null;
        }

        public ValueProviderResult GetValue(string key)
        {
            var formData = _httpContext.Request.Form[0];
            return new ValueProviderResult(formData, formData, CultureInfo.CurrentCulture);
        }
    }
}
