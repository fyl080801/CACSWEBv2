using CACS.Framework.Data;
using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Mvc;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Profiles;
using CACSLibrary.Configuration;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using CACSLibrary.Profile;
using CACSLibrary.Web;
using CACSLibrary.Web.EmbeddedViews;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(CACS.WebSite.OwinStartup))]
namespace CACS.WebSite
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            IEngine engine = new CACSWebEngine();
            EngineContext.Reset(engine);
            CACSConfig config = ConfigurationManager.GetSection("cacsConfig") as CACSConfig;
            EngineContext.Current.Initialize(config);

            DependencyResolver.SetResolver(new CACSDependencyResolver());

            ModelBinders.Binders.Add(typeof(BaseModel), new CACSModelBinder());
            ControllerBuilder.Current.SetControllerFactory(new CACSControllerFactory());

            ViewConfig.Register(new EmbeddedViewVirtualPathProvider(EngineContext.Current.Resolve<IEmbeddedViewResolver>().GetEmbeddedViews()));

            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            var globalProfile = EngineContext.Current.Resolve<IProfileManager>().Get<GlobalSettings>();

            app.CreatePerOwinContext(CreateDbContext);
            app.CreatePerOwinContext<ApplicationUserManager>(CreateUserManager);
            app.CreatePerOwinContext<ApplicationRoleManager>((options, context) =>
                new ApplicationRoleManager(new IntRoleStore(context.Get<DbContext>())));
            app.CreatePerOwinContext<ApplicationSignInManager>((options, context) =>
                new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication));

            // cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(globalProfile.LoginUrl),
                Provider = new CookieAuthenticationProvider
                {
                    // 当用户登录时使应用程序可以验证安全戳。
                    // 这是一项安全功能，当你更改密码或者向帐户添加外部登录名时，将使用此功能。
                    //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User, int>(
                    //    validateInterval: TimeSpan.FromMinutes(globalProfile.LoginTimeout),
                    //    regenerateIdentityCallback: (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
                    //    getUserIdCallback: (claims) =>
                    //    {
                    //        var id = claims.GetUserId<int>();
                    //        return id;
                    //    })
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 使应用程序可以在双重身份验证过程中验证第二因素时暂时存储用户信息。
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(globalProfile.LoginTimeout));

            // 使应用程序可以记住第二登录验证因素，例如电话或电子邮件。
            // 选中此选项后，登录过程中执行的第二个验证步骤将保存到你登录时所在的设备上。
            // 此选项类似于在登录时提供的“记住我”选项。
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }

        private DbContext CreateDbContext()
        {
            return EngineContext.Current.Resolve<IDbContext>() as DbContext;
        }

        private ApplicationUserManager CreateUserManager(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new IntUserStore(context.Get<DbContext>()));

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