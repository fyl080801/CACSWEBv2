using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.WebSite.Models.Account;
using CACSLibrary;
using CACSLibrary.Data;
using CACSLibrary.Profile;
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

        public UserController(
            IProfileManager profileManager,
            HttpContextBase httpContext)
        {
            _profileManager = profileManager;
            _userManager = httpContext.GetOwinContext().Get<ApplicationUserManager>();
        }

        [AccountTicket(Group = "用户管理", AuthorizeName = "浏览")]
        public ActionResult List()
        {
            var query = _userManager.Users;
            return JsonList(query.Select(UserInfo.Prepare).ToArray());
        }


        public async Task<ActionResult> Details(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            return Json(UserInfo.Prepare(result));
        }

        [AccountTicket(AuthorizeName = "删除", Group = "用户管理")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new CACSException("找不到用户");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new CACSException(string.Join(", ", result.Errors.ToArray()));
            return Json(result.Succeeded);
        }

        [AccountTicket(AuthorizeName = "编辑", Group = "用户管理"), HttpPost]
        public async Task<ActionResult> Update(UserInfo model)
        {
            var result = await _userManager.UpdateAsync(model.ToDomain());
            if (!result.Succeeded)
                throw new CACSException(string.Join(", ", result.Errors.ToArray()));
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeName = "创建", Group = "用户管理"), HttpPost]
        public async Task<ActionResult> Create(UserInfo model)
        {
            var user = new User()
            {
                Id = model.Id,
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user);
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