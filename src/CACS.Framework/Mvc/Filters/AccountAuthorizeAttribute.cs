using CACS.Framework.Domain;
using CACS.Framework.Mvc.ActionResults;
using CACS.Framework.Profiles;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;
using System.Linq;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.Filters
{
    public class AccountAuthorizeAttribute : AccountTicketAttribute
    {
        IRepository<Rule> _ruleRepository;
        IProfileManager _profileManager;

        public string SystemId
        {
            get;
            set;
        }

        public string Group
        {
            get;
            private set;
        }

        public string Action
        {
            get;
            private set;
        }

        public string AuthorizeName
        {
            get { return string.Format("{0}_{1}_{2}", SystemId, Group, Action); }
        }

        public AccountAuthorizeAttribute(string group, string action)
        {
            _ruleRepository = EngineContext.Current.Resolve<IRepository<Rule>>();
            _profileManager = EngineContext.Current.Resolve<IProfileManager>();
            Group = group;
            Action = action;
        }

        protected override void HandleAccountRequest(AuthorizationContext filterContext)
        {
            base.HandleAccountRequest(filterContext);

            if (!IsAuthenticated)
                return;

            if (filterContext.HttpContext.User.IsInRole("Administrators"))
                return;

            if (string.IsNullOrEmpty(SystemId))
            {
                var systemInfo = _profileManager.Get<SystemInformation>();
                SystemId = systemInfo.SystemId;
            }

            var rules = _ruleRepository.Table.Where(e => e.AuthorizeName == AuthorizeName);
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
                filterContext.Result = new UnAuthorized(Group, Action);
        }
    }
}
