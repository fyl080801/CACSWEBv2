using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Interfaces;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Mvc.Models;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System;

namespace CACS.Services
{
    public class AccountService : IAccountService
    {
        ITypeFinder _typeFinder;
        IRepository<Rule> _ruleRepository;
        ApplicationUserManager _userManager;
        ApplicationRoleManager _roleManager;

        public AccountService(
            ITypeFinder typeFinder,
            IRepository<Rule> ruleRepository,
            ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            _typeFinder = typeFinder;
            _ruleRepository = ruleRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ICollection<AuthorizeModel> GetAuthorizes()
        {
            var list = new Dictionary<string, AuthorizeModel>();
            var controllers = _typeFinder.FindClassesOfType<ControllerBase>();
            foreach (var controller in controllers)
            {
                var controllerMethods = controller.GetMethods();
                foreach (var method in controllerMethods)
                {
                    var ticketAttribute = method.GetCustomAttribute<AccountTicketAttribute>();
                    if (ticketAttribute != null && !string.IsNullOrEmpty(ticketAttribute.AuthorizeName))
                    {
                        var authorizeId = !string.IsNullOrEmpty(ticketAttribute.AuthorizeId) ? ticketAttribute.AuthorizeId : AccountTicketAttribute.BuildAuthorizeId(
                                 controller.Namespace,
                                 Regex.Replace(controller.Name, "(Controller)$", ""),
                                 method.Name);
                        if (!list.ContainsKey(authorizeId))
                        {
                            list.Add(authorizeId, new AuthorizeModel()
                            {
                                AuthorizeName = ticketAttribute.AuthorizeName,
                                Group = ticketAttribute.Group,
                                Id = authorizeId
                            });
                        }
                        else
                        {
                            list[authorizeId].AuthorizeName = !string.IsNullOrEmpty(ticketAttribute.AuthorizeName) ? ticketAttribute.AuthorizeName : list[authorizeId].AuthorizeName;
                            list[authorizeId].Group = !string.IsNullOrEmpty(ticketAttribute.Group) ? ticketAttribute.Group : list[authorizeId].Group;
                        }
                    }
                }
            }
            return list.Values;
        }

        public ICollection<RoleAuthorizeModel> GetRoleAuthorizes(int id)
        {
            var list = new List<RoleAuthorizeModel>();
            var rules = _ruleRepository.Table.Where(e => e.RoleId == id).ToArray();
            var auths = GetAuthorizes();
            foreach (var auth in auths)
            {
                list.Add(new RoleAuthorizeModel(auth)
                {
                    RoleId = id,
                    IsAuthorized = rules.FirstOrDefault(e => e.AuthorizeId == auth.Id) != null
                });
            }
            return list;
        }

        public ICollection<string> GetUserAuthorizes(int id)
        {
            var user = _userManager.Users.FirstOrDefault(e => e.Id == id);
            var administrators = _roleManager.Roles.FirstOrDefault(e => e.Name == ApplicationRoleManager.Administrators);
            if (user != null && administrators != null)
            {
                var roles = user.Roles.Select(e => e.RoleId).ToArray();
                if (roles.Contains(administrators.Id))
                {
                    return GetAuthorizes().Select(e => e.Id).ToArray();
                }
                else
                {
                    var roleRules = _ruleRepository.Table.Where(e => roles.Contains(e.RoleId));
                    return roleRules.Select(e => e.AuthorizeId).ToArray();
                }
            }
            return new string[0];
        }

        public void SetRoleAuthorizes(AuthorizeModel[] authorizes, int roleId)
        {
            var rules = _ruleRepository.Table.Where(e => e.RoleId == roleId).ToArray();
            _ruleRepository.Delete(rules);
            _ruleRepository.Insert(authorizes.Select(e => new Rule()
            {
                AuthorizeId = e.Id,
                RoleId = roleId
            }).ToArray());
        }
    }
}
