using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Interfaces;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Mvc.Models;
using CACS.WebSite.Models.Account;
using CACSLibrary;
using CACSLibrary.Data;
using CACSLibrary.Profile;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CACS.WebSite.Controllers
{
    public class RoleController : CACSController
    {
        IProfileManager _profileManager;
        ApplicationRoleManager _roleManager;
        ApplicationUserManager _userManager;
        IAccountService _accountService;

        public RoleController(
            IProfileManager profileManager,
            IAccountService accountService,
            HttpContextBase httpContext)
        {
            _profileManager = profileManager;
            _accountService = accountService;
            _roleManager = httpContext.GetOwinContext().Get<ApplicationRoleManager>();
            _userManager = httpContext.GetOwinContext().Get<ApplicationUserManager>();
        }

        public ActionResult List()
        {
            var query = _roleManager.Roles;
            return JsonList(query.Select(RoleModel.Prepare).ToArray());
        }

        //[AccountTicket(AuthorizeName = "创建", Group = "角色管理"), HttpPost, Logging("创建角色")]
        //public async Task<ActionResult> Create(RoleModel model)
        //{
        //    var role = new Role()
        //    {
        //        Name = model.RoleName,
        //        Description = model.Description
        //    };
        //    var result = await _roleManager.CreateAsync(role);
        //    if (!result.Succeeded)
        //        throw new CACSException(string.Join(", ", result.Errors.ToArray()));
        //    return Json(role.Id);
        //}

        //[AccountTicket(AuthorizeName = "编辑", Group = "角色管理"), HttpPost]
        //public async Task<ActionResult> Update(RoleModel model)
        //{
        //    var result = await _roleManager.UpdateAsync(model.ToDomain());
        //    if (!result.Succeeded)
        //        throw new CACSException(string.Join(", ", result.Errors.ToArray()));
        //    return Json(model.Id);
        //}

        [AccountTicket(AuthorizeName = "编辑", Group = "角色管理")]
        public async Task<ActionResult> Save(RoleModel model)
        {
            var old = await _roleManager.FindByIdAsync(model.Id);
            IdentityResult result;
            if (old != null)
            {
                if (old.Name != ApplicationRoleManager.Administrators)
                    old.Name = model.Name;
                old.Description = model.Description;
                result = await _roleManager.UpdateAsync(old);
            }
            else
            {
                old = model.ToDomain();
                result = await _roleManager.CreateAsync(old);
            }
            if (!result.Succeeded)
                throw new CACSException(string.Join(", ", result.Errors.ToArray()));
            return Json(old.Id);
        }

        [AccountTicket(AuthorizeName = "删除", Group = "角色管理")]
        public async Task<ActionResult> Delete(int id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new CACSException("找不到角色");
            if (role.Name == ApplicationRoleManager.Administrators)
                throw new CACSException("管理员角色不能删除");

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                throw new CACSException(string.Join(", ", result.Errors.ToArray()));
            return Json(result.Succeeded);
        }

        [AccountTicket]
        public async Task<ActionResult> Details(int id)
        {
            var result = await _roleManager.FindByIdAsync(id);
            return Json(RoleModel.Prepare(result));
        }

        [AccountTicket]
        public ActionResult LoadRoles(string keyword, int limit, int page, string sort, string dir)
        {
            IDictionary<string, bool> dic = new Dictionary<string, bool>();
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(dir))
            {
                dic.Add(sort, dir == "ASC" ? true : false);
            }

            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(m => m.Name.Contains(keyword) || m.Description.Contains(keyword));
            foreach (var item in dic)
            {
                query = QueryBuilder.DataSorting(query, item.Key, item.Value);
            }
            var roles = new PagedList<Role>(query.ToList(), page - 1, limit);
            if (roles == null)
            {
                roles = new PagedList<Role>(new List<Role>(), page - 1, limit);
            }
            return JsonList(roles.Select(RoleModel.Prepare).ToArray(), roles.TotalCount);
        }

        [AccountTicket(AuthorizeName = "浏览", Group = "角色管理")]
        public ActionResult Roles(ListModel model)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(model.Search))
                query = query.Where(m => m.Name.Contains(model.Search) || m.Description.Contains(model.Search));
            if (model.Sort.Count > 0)
            {
                model.Sort.ForEach(sortItem =>
                {
                    query = QueryBuilder.DataSorting(
                        query,
                        sortItem.Key,
                        sortItem.Value.Equals("asc", StringComparison.InvariantCultureIgnoreCase) ? true : false);
                });
            }
            else
            {
                query = query.OrderBy(m => m.Id);
            }
            var roles = new PagedList<Role>(query, model.Page - 1, model.Limit);
            if (roles == null)
            {
                roles = new PagedList<Role>(new List<Role>(), model.Page - 1, model.Limit);
            }
            return JsonList(roles.Select(RoleModel.Prepare).ToArray(), roles.TotalCount);
        }

        [AccountTicket]
        public ActionResult Authorizes(int id)
        {
            var roleAuthorizes = _accountService.GetRoleAuthorizes(id);
            return JsonList<RoleAuthorizeModel>(roleAuthorizes.ToArray());
        }

        [AccountTicket(AuthorizeId = "/Role/Save")]
        public ActionResult SetAuthorizes(RoleAuthorizeModel[] models)
        {
            var roles = models.Select(e => e.RoleId).Distinct().ToArray();
            foreach (var roleId in roles)
            {
                _accountService.SetRoleAuthorizes(models.Where(e => e.IsAuthorized == true).Select(a => new AuthorizeModel()
                {
                    AuthorizeName = a.AuthorizeName,
                    Group = a.Group,
                    Id = a.Id
                }).ToArray(), roleId);
            }
            return Json(true);
        }

        [AccountTicket]
        public ActionResult RoleMembers(ListModel model, int id)
        {
            var role = _roleManager.FindById(id);
            if (role == null)
                throw new CACSException("找不到角色");
            var roleusers = role.Users.Select(r => r.UserId).ToArray();
            var query = _userManager.Users.Where(e => roleusers.Contains(e.Id));
            if (!string.IsNullOrEmpty(model.Search))
                query = query.Where(m => m.FirstName.Contains(model.Search) || m.LastName.Contains(model.Search));
            if (model.Sort.Count > 0)
            {
                model.Sort.ForEach(sortItem =>
                {
                    var order = sortItem.Value.Equals("asc", StringComparison.InvariantCultureIgnoreCase) ? true : false;
                    var key = sortItem.Key == "Username" ? "UserName" : sortItem.Key;
                    if (key == "PersonalName")
                    {
                        query = QueryBuilder.DataSorting(query, "LastName", order);
                        query = QueryBuilder.DataSorting(query, "FirstName", order);
                    }
                    else
                    {
                        query = QueryBuilder.DataSorting(query, key, order);
                    }
                });
            }
            else
            {
                query = query.OrderBy(m => m.LastName);
            }
            var result = new PagedList<User>(query, model.Page - 1, model.Limit) ??
                new PagedList<User>(new List<User>(), model.Page - 1, model.Limit);
            return JsonList(result.Select(UserInfo.Prepare).ToArray(), result.TotalCount);
        }

        [AccountTicket(AuthorizeId = "/Role/Save")]
        public ActionResult Members(ListModel model, int id)
        {
            var role = _roleManager.FindById(id);
            if (role == null)
                throw new CACSException("找不到角色");
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(model.Search))
                query = query.Where(m => m.FirstName.Contains(model.Search) || m.LastName.Contains(model.Search));
            if (model.Sort.Count > 0)
            {
                model.Sort.ForEach(sortItem =>
                {
                    var order = sortItem.Value.Equals("asc", StringComparison.InvariantCultureIgnoreCase) ? true : false;
                    if (sortItem.Key == "PersonalName")
                    {
                        query = QueryBuilder.DataSorting(query, "LastName", order);
                        query = QueryBuilder.DataSorting(query, "FirstName", order);
                    }
                    else
                    {
                        query = QueryBuilder.DataSorting(query, sortItem.Key, order);
                    }
                });
            }
            else
            {
                query = query.OrderBy(m => m.LastName);
            }
            var result = new PagedList<User>(query, model.Page - 1, model.Limit) ??
                new PagedList<User>(new List<User>(), model.Page - 1, model.Limit);
            return JsonList(result
                .Select(m => new RoleMemberModel()
                {
                    Id = m.Id,
                    IsMember = m.Roles.Select(r => r.RoleId).ToArray().Contains(id),
                    UserName = m.UserName,
                    PersonalName = m.PersonalName,
                    RoleId = id
                }).ToArray(), result.TotalCount);
        }

        [AccountTicket(AuthorizeId = "/Role/Save")]
        public ActionResult SetMembers(RoleMemberModel[] models, int id)
        {
            var domain = _roleManager.FindById(id);

            var users = domain.Users.Select(e => e.UserId);
            var newMembers = models.Where(e => e.IsMember == true && !users.Contains(e.Id));
            var removedMembers = domain.Users.Where(e => models.Where(x => x.IsMember == false).Select(x => x.Id).ToArray().Contains(e.UserId));//models.Where(e => e.IsMember == false && users.Contains(e.Id));

            newMembers.ForEach(e => domain.Users.Add(new UserRole() { UserId = e.Id, RoleId = id }));
            removedMembers.ToList().ForEach(e => domain.Users.Remove(e));

            if (domain.Name == ApplicationRoleManager.Administrators && domain.Users.Count <= 0)
                throw new CACSException("超级管理员至少要包含一个用户");
            _roleManager.Update(domain);
            return Json(id);
        }
    }
}