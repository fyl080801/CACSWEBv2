using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.ActionResults
{
    public class CACSJsonResult : JsonResult
    {
        bool _ignoreReferenceLoop;

        public CACSJsonResult() { }

        public CACSJsonResult(bool ignoreReferenceLoop)
        {
            _ignoreReferenceLoop = ignoreReferenceLoop;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet
                && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JsonRequest_GetNotAllowed");
            HttpResponseBase response = context.HttpContext.Response;

            if (!string.IsNullOrEmpty(this.ContentType))
                response.ContentType = this.ContentType;
            else
                response.ContentType = "application/json";

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                //DateTimeConverterBase timeFormat = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                JsonSerializerSettings setting = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = _ignoreReferenceLoop ? ReferenceLoopHandling.Ignore : ReferenceLoopHandling.Serialize,
                };
                //setting.Converters.Add(timeFormat);
                response.Write(JsonConvert.SerializeObject(this.Data, Formatting.Indented, setting));
            }
        }
    }
}
