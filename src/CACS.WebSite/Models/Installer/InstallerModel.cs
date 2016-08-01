using CACS.Framework.Profiles;
using CACSLibrary.Profile;
using System.ComponentModel.DataAnnotations;

namespace CACS.WebSite.Models.Installer
{
    public class InstallerModel : BaseProfile
    {
        [Required]
        public string DatabaseInstance { get; set; }

        [Required]
        public string DatabaseName { get; set; }

        [Required]
        public string DatabaseAccount { get; set; }

        [Required]
        public string DatabasePassword { get; set; }

        [Required]
        public string AdminPassword { get; set; }

        [Required, Compare("AdminPassword", ErrorMessage = "确认密码不一致")]
        public string ConfirmPassword { get; set; }

        [Required, StringLength(50, ErrorMessage = "长度不能超过50")]
        public string AdminEmail { get; set; }

        public string DatabaseConnectionString
        {
            get
            {
                return string.Format(
                    "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};MultipleActiveResultSets=true;",
                    this.DatabaseInstance,
                    this.DatabaseName,
                    this.DatabaseAccount,
                    this.DatabasePassword);
            }
        }

        public override ProfileObject GetDefault()
        {
            return new InstallerModel()
            {
                DatabaseAccount = "sa",
                DatabaseInstance = ".",
                DatabasePassword = "qazwsxedc",
                AdminEmail = "admin@cacs.com.cn",
                AdminPassword = "123",
            };
        }
    }
}