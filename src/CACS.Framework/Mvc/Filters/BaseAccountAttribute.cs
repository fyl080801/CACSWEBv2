using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public abstract class BaseAccountAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
                throw new InvalidOperationException("AccountAttribute 不能应用到子行为上");

            if (IsAccountPageRequested(filterContext))
            {
                HandleAccountRequest(filterContext);
            }
        }

        private IEnumerable<BaseAccountAttribute> GetAccountAuthorizeAttributes(ActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes(typeof(BaseAccountAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof(BaseAccountAttribute), true))
                .OfType<BaseAccountAttribute>();
        }

        private bool IsAccountPageRequested(AuthorizationContext filterContext)
        {
            var adminAttributes = GetAccountAuthorizeAttributes(filterContext.ActionDescriptor);
            if (adminAttributes != null && adminAttributes.Any())
                return true;
            return false;
        }

        protected abstract void HandleAccountRequest(AuthorizationContext filterContext);
    }
}
