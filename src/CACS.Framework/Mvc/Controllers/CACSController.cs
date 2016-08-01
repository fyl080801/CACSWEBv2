using CACS.Framework.Mvc.ActionResults;
using System.Text;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.Controllers
{
    public class CACSController : Controller
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new CACSJsonResult()
            {
                Data = data,
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                JsonRequestBehavior = behavior
            };
        }

        protected JsonResult JsonList<T>(T[] data, int total)
        {
            return this.Json(
                new JsonListData<T>(true, data, total),
                "text/html",
                System.Text.Encoding.Unicode,
                JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JsonList<T>(T[] data)
        {
            return this.Json(
                new JsonListData<T>(true, data),
                "text/html",
                System.Text.Encoding.Unicode,
                JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Json<T>(T data)
        {
            return this.Json(
                new JsonContentData<T>(data, true, ""),
                "text/html",
                System.Text.Encoding.Unicode,
                JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Json<T>(bool success, T data, string message)
        {
            return this.Json(
                new JsonContentData<T>(data, success, message),
                "text/html",
                System.Text.Encoding.Unicode,
                JsonRequestBehavior.AllowGet);
        }
    }
}
