using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Mvc.ActionResults;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace CACS.Framework.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class AccountTicketAttribute : BaseAccountAttribute
    {
        IRepository<Rule> _ruleRepository;
        IProfileManager _profileManager;

        public string Group { get; set; } = "系统";

        public string AuthorizeName { get; set; }

        public string AuthorizeId { get; set; }

        public AccountTicketAttribute()
        {
            _ruleRepository = EngineContext.Current.Resolve<IRepository<Rule>>();
            _profileManager = EngineContext.Current.Resolve<IProfileManager>();
        }

        protected override void HandleAccountRequest(AuthorizationContext filterContext)
        {
            if (!HasAccountAccess(filterContext))
            {
                HandleUnValidatedRequest(filterContext);
            }
            else if (!filterContext.HttpContext.User.IsInRole(ApplicationRoleManager.Administrators))
            {
                string authorizeId = !string.IsNullOrEmpty(this.AuthorizeId) ? this.AuthorizeId : BuildAuthorizeId(
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace,
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName);
                var rules = _ruleRepository.Table.Where(e => e.AuthorizeId == authorizeId).ToArray();
                bool isAuthorized = false;
                foreach (var rule in rules)
                {
                    isAuthorized = filterContext.HttpContext.User.IsInRole(rule.Role.Name);
                    if (isAuthorized)
                    {
                        break;
                    }
                }
                if (!isAuthorized)
                    HandleUnUnAuthorizedRequest(filterContext);
            }
        }

        protected bool HasAccountAccess(AuthorizationContext filterContext)
        {
            return filterContext.HttpContext.User.Identity.IsAuthenticated;
        }

        private void HandleUnValidatedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new UnValidated();
        }

        private void HandleUnUnAuthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new UnAuthorized(
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName);
        }

        public static string BuildAuthorizeId(string namespaces, string controller, string action)
        {
            var routes = RouteTable.Routes.Where(e => e.GetType() == typeof(Route));
            routes = routes.Where(e => ((Route)e).DataTokens != null && ((Route)e).DataTokens.ContainsKey("Namespaces"));
            var routeCollection = routes.Select(e => e as Route);
            var area = "";
            namespaces = NamespaceReg(namespaces);
            foreach (var route in routeCollection)
            {
                var routeNamespaces = route.DataTokens["Namespaces"] as string[];
                var areamatch = routeNamespaces.FirstOrDefault(e => IsNamespaceMatch(e, namespaces));
                if (areamatch != null && route.DataTokens.ContainsKey("area"))
                {
                    area = route.DataTokens["area"].ToString();
                    break;
                }
            }
            return string.Format("{3}{0}/{1}/{2}", area, controller, action, string.IsNullOrEmpty(area) ? "" : "/");
        }

        private static string NamespaceReg(string type)
        {
            var regType = new Regex(@".*\.(.*\.)?");
            MatchCollection matches = regType.Matches(type);
            var match = matches[0].Value.Substring(0, matches[0].Value.Length - 1);
            return Regex.Replace(match, @"\.", "\\.");
        }

        private static bool IsNamespaceMatch(string path, string type)
        {
            var regStr = string.Format("^{0}(\\.[^0-9](\\w*))?$", type);
            Regex reg = new Regex(regStr);
            return reg.IsMatch(path);
        }
    }
}
