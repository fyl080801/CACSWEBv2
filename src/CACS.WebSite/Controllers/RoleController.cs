using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.WebSite.Models.Account;
using CACSLibrary;
using CACSLibrary.Data;
using CACSLibrary.Profile;
using Microsoft.AspNet.Identity.Owin;
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

        public RoleController(
            IProfileManager profileManager,
            HttpContextBase httpContext)
        {
            _profileManager = profileManager;
            _roleManager = httpContext.GetOwinContext().Get<ApplicationRoleManager>();
        }

        [AccountTicket(AuthorizeName = "浏览", Group = "角色管理")]
        public ActionResult List()
        {
            var query = _roleManager.Roles;
            return JsonList(query.Select(RoleModel.Prepare).ToArray());
        }

        [AccountTicket(AuthorizeName = "创建", Group = "角色管理"), HttpPost, Logging("创建角色")]
        public async Task<ActionResult> Create(RoleModel model)
        {
            var role = new Role()
            {
                Name = model.RoleName,
                Description = model.Description
            };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new CACSException(string.Join(", ", result.Errors.ToArray()));
            return Json(role.Id);
        }

        [AccountTicket(AuthorizeName = "编辑", Group = "角色管理"), HttpPost]
        public async Task<ActionResult> Update(RoleModel model)
        {
            var result = await _roleManager.UpdateAsync(model.ToDomain());
            if (!result.Succeeded)
                throw new CACSException(string.Join(", ", result.Errors.ToArray()));
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeName = "删除", Group = "角色管理")]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new CACSException("找不到角色");

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                throw new CACSException(string.Join(", ", result.Errors.ToArray()));
            return Json(result.Succeeded);
        }

        [AccountTicket]
        public async Task<ActionResult> Details(string id)
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
    }
}