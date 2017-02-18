using CACS.Framework.Domain;
using CACS.Framework.Profiles;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACS.Framework.Identity
{
    public static class IdentityExtensions
    {
        public static IAppBuilder CreateIdentityExtensions(this IAppBuilder app, IEngine engine)
        {
            var globalProfile = engine.Resolve<IProfileManager>().Get<GlobalSettings>();
            app.CreatePerOwinContext<DbContext>((options, context) =>
                EngineContext.Current.Resolve<IDbContext>() as DbContext);
            app.CreatePerOwinContext<ApplicationUserStore>((options, context) =>
                new ApplicationUserStore(context.Get<DbContext>()));
            app.CreatePerOwinContext<ApplicationUserManager>(CreateUserManager);
            app.CreatePerOwinContext<ApplicationRoleManager>((options, context) =>
                new ApplicationRoleManager(new ApplicationRoleStore(context.Get<DbContext>())));
            app.CreatePerOwinContext<ApplicationSignInManager>((options, context) =>
                new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication));

            // cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(globalProfile.LoginUrl),
                ExpireTimeSpan = TimeSpan.FromMinutes(globalProfile.LoginTimeout),
                Provider = new CookieAuthenticationProvider
                {
                    // 当用户登录时使应用程序可以验证安全戳。
                    // 这是一项安全功能，当你更改密码或者向帐户添加外部登录名时，将使用此功能。
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User, int>(
                        validateInterval: TimeSpan.FromMinutes(globalProfile.LoginTimeout),
                        regenerateIdentityCallback: (manager, user) =>
                        {
                            return user.GenerateUserIdentityAsync(manager);
                        },
                        getUserIdCallback: (claim) =>
                        {
                            return int.Parse(claim.GetUserId());
                        })
                },
                CookieName = ".CACSWEB"
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 使应用程序可以在双重身份验证过程中验证第二因素时暂时存储用户信息。
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(globalProfile.LoginTimeout));

            // 使应用程序可以记住第二登录验证因素，例如电话或电子邮件。
            // 选中此选项后，登录过程中执行的第二个验证步骤将保存到你登录时所在的设备上。
            // 此选项类似于在登录时提供的“记住我”选项。
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            return app;
        }

        private static ApplicationUserManager CreateUserManager(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(context.Get<ApplicationUserStore>());

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并将其插入到此处。
            manager.RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<User, int>
            {
                MessageFormat = "你的安全代码是 {0}"
            });
            manager.RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<User, int>
            {
                Subject = "安全代码",
                BodyFormat = "你的安全代码是 {0}"
            });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
