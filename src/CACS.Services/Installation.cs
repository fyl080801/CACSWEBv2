using CACS.Framework.Data;
using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Interfaces;
using CACS.Framework.Profiles;
using CACSLibrary.Profile;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace CACS.Services
{
    public class Installation : MarshalByRefObject, IInstallation
    {
        IProfileManager _profileManager;
        IDataSettingsManager _dataSettingManager;
        DbContext _dbContext;
        ApplicationUserManager _userManager;
        ApplicationRoleManager _roleManager;
        SystemInformation _systemInfo;

        public Installation(
            IProfileManager profileManager,
            IDataSettingsManager dataSettingManager)
        {
            _profileManager = profileManager;
            _dataSettingManager = dataSettingManager;
        }

        public void InstallData(string adminPassword, string adminMail, string systemMail)
        {
            InstallSystemInformation();
            InstallUserRole(adminPassword, adminMail, systemMail);
        }

        private void InstallUserRole(string adminPassword, string adminMail, string systemMail)
        {
            var admin = new User
            {
                UserName = ApplicationUserManager.Administrator,
                Email = adminMail,
                LastName = "Administrator"
            };
            var role = new Role()
            {
                Name = ApplicationRoleManager.Administrators
            };

            _dbContext = new CACSWebObjectContext(_dataSettingManager.LoadSettings().ConnectionString);
            _roleManager = new ApplicationRoleManager(new RoleStore<Role>(_dbContext));
            _userManager = new ApplicationUserManager(new UserStore<User>(_dbContext));

            _roleManager.Create(role);
            _userManager.Create(admin, adminPassword);
            _userManager.AddToRole(admin.Id, role.Name);
        }

        private void InstallSystemInformation()
        {
            _profileManager.Add<SystemInformation>();
            _systemInfo = _profileManager.Get<SystemInformation>();
            _systemInfo.InstallTime = DateTime.Now;
            _profileManager.Save(_systemInfo);
        }
    }
}
