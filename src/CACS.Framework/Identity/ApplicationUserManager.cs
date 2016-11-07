using CACS.Framework.Data;
using CACS.Framework.Domain;
using CACSLibrary;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CACS.Framework.Identity
{
    public class ApplicationUserManager : UserManager<User, int>
    {
        public const string Administrator = "admin";

        public ApplicationUserManager(IUserStore<User, int> store)
            : base(store)
        {
            var settings = EngineContext.Current.Resolve<IProfileManager>().Get<IdentitySettings>();
            this.UserValidator = new UserValidator<User, int>(this)
            {
                AllowOnlyAlphanumericUserNames = settings.AllowOnlyAlphanumericUserNames,
                RequireUniqueEmail = settings.RequireUniqueEmail
            };

            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = settings.RequiredLength,
                RequireNonLetterOrDigit = settings.RequireNonLetterOrDigit,
                RequireDigit = settings.RequireDigit,
                RequireLowercase = settings.RequireLowercase,
                RequireUppercase = settings.RequireUppercase,
            };
        }

        public override Task<IdentityResult> DeleteAsync(User user)
        {
            var olduser = this.FindById(user.Id);
            if (olduser.UserName == Administrator)
                throw new CACSException("管理员账户不能删除");
            return base.DeleteAsync(olduser);
        }
    }
}
