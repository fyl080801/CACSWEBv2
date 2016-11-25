using CACS.Framework.Domain;
using CACS.Framework.Identity;
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
    public class UserController : CACSController
    {
        IProfileManager _profileManager;
        ApplicationUserManager _userManager;
        ApplicationRoleManager _roleManager;

        public UserController(
            IProfileManager profileManager,
            HttpContextBase httpContext)
        {
            _profileManager = profileManager;
            _userManager = httpContext.GetOwinContext().Get<ApplicationUserManager>();
            _roleManager = httpContext.GetOwinContext().Get<ApplicationRoleManager>();
        }

        [AccountTicket(Group = "用户管理", AuthorizeName = "浏览")]
        public ActionResult List()
        {
            var query = _userManager.Users;
            return JsonList(query.Select(UserInfo.Prepare).ToArray());
        }


        public async Task<ActionResult> Details(int id)
        {
            var result = await _userManager.FindByIdAsync(id);
            return Json(UserInfo.Prepare(result));
        }

        [AccountTicket(AuthorizeName = "删除", Group = "用户管理")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new CACSException("找不到用户");
            var adminRole = _roleManager.FindByName(ApplicationRoleManager.Administrators);
            if (adminRole.Users.Count <= 1 && user.Id == adminRole.Users.FirstOrDefault().UserId)
                throw new CACSException("必须保留至少一个管理员账户");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new CACSException(string.Join(", ", result.Errors.ToArray()));
            return Json(result.Succeeded);
        }

        [AccountTicket(AuthorizeName = "保存", Group = "用户管理"), HttpPost]
        public async Task<ActionResult> Save(UserInfo model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            IdentityResult result;
            if (user != null)
            {
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                result = await _userManager.UpdateAsync(user);
            }
            else
            {
                user = model.ToDomain();
                result = await _userManager.CreateAsync(user, user.UserName);
            }
            if (!result.Succeeded)
                throw new CACSException(string.Join(", ", result.Errors.ToArray()));
            return Json(user.Id);
        }

        [AccountTicket]
        public ActionResult GetUsers(UserListModel model, int limit, int page, string sort, string dir)
        {
            IDictionary<string, bool> dic = new Dictionary<string, bool>();
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(dir))
            {
                dic.Add(sort, dir == "ASC" ? true : false);
            }
            var query = _userManager.Users;
            if (!String.IsNullOrWhiteSpace(model.SearchEmail))
                query = query.Where(c => c.Email.Contains(model.SearchEmail));
            if (!String.IsNullOrWhiteSpace(model.SearchUserName))
                query = query.Where(c => c.UserName.Contains(model.SearchUserName));
            if (!string.IsNullOrWhiteSpace(model.SearchPersonalName))
                query = query.Where(c => c.PersonalName.Contains(model.SearchPersonalName));
            if (!string.IsNullOrWhiteSpace(model.SearchKeyword))
                query = query.Where(c => c.UserName.Contains(model.SearchKeyword) || c.PersonalName.Contains(model.SearchKeyword) || c.Email.Contains(model.SearchKeyword));

            if (dic != null && dic.Count > 0)
            {
                foreach (var sortItem in dic)
                {
                    query = QueryBuilder.DataSorting(query, sortItem.Key, sortItem.Value);
                }
            }
            else
            {
                query = query.OrderByDescending(m => m.LockoutEndDateUtc);
            }
            var users = new PagedList<UserInfo>(query, page - 1, limit);
            if (users == null)
            {
                users = new PagedList<UserInfo>(new List<UserInfo>(), page - 1, limit);
            }
            return JsonList(
                users.ToList().ConvertAll<UserInfo>(item => new UserInfo()
                {
                    Id = item.Id,
                    Username = item.Username,
                    PersonalName = item.PersonalName,
                }).ToArray(),
                users.TotalCount);
        }

        [AccountTicket]
        public ActionResult Users(ListModel model)
        {
            var query = _userManager.Users;
            if (!String.IsNullOrWhiteSpace(model.Search))
                query = query.Where(c => c.UserName.Contains(model.Search) || c.FirstName.Contains(model.Search) || c.LastName.Contains(model.Search));

            if (model.Sort.Count > 0)
            {
                model.Sort.ForEach(sortItem =>
                {
                    string key = sortItem.Key;
                    if (sortItem.Key == "Username")
                    {
                        key = "UserName";
                    }
                    else if (sortItem.Key == "PersonalName")
                    {
                        key = "LastName";
                    }
                    query = QueryBuilder.DataSorting(
                        query,
                        key,
                        sortItem.Value.Equals("asc", StringComparison.InvariantCultureIgnoreCase) ? true : false);
                });
            }
            else
            {
                query = query.OrderByDescending(m => m.LockoutEndDateUtc);
            }
            var users = new PagedList<User>(query, model.Page - 1, model.Limit);
            if (users == null)
            {
                users = new PagedList<User>(new List<User>(), model.Page - 1, model.Limit);
            }
            return JsonList(
                users.ToList().ConvertAll<UserInfo>(item => new UserInfo()
                {
                    Id = item.Id,
                    Username = item.UserName,
                    PersonalName = item.PersonalName,
                }).ToArray(),
                users.TotalCount);
        }

        //public ActionResult SelectList(UserSelectModel model, int limit, int page, string sort, string dir)
        //{
        //    IDictionary<string, bool> dic = new Dictionary<string, bool>();
        //    if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(dir))
        //    {
        //        dic.Add(sort, dir == "ASC" ? true : false);
        //    }
        //    var users = _userService.GetUserByFirst(
        //        model.SeachFirstName,
        //        page - 1,
        //        limit,
        //        dic);
        //    return JsonList<UserModel>(
        //        users.Select(UserModel.Prepare).ToArray(),
        //        users.TotalCount);
        //}
    }
}