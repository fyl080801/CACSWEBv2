using System.Web.Mvc;

namespace CACS.Framework.Mvc.ActionResults
{
    public abstract class CACSResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            ActionResult result = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? GetAjaxResult(context) : GetRequestResult(context);
            result.ExecuteResult(context);
        }

        protected abstract ActionResult GetRequestResult(ControllerContext context);

        protected abstract ActionResult GetAjaxResult(ControllerContext context);
    }
}
