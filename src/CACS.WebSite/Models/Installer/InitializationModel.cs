using CACS.Framework.Profiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CACSLibrary.Profile;

namespace CACS.WebSite.Models.Installer
{
    public class InitializationModel : BaseProfile
    {
        [Required]
        public string AdminPassword { get; set; }

        [Required, Compare("AdminPassword", ErrorMessage = "确认密码不一致")]
        public string ConfirmPassword { get; set; }

        [Required, StringLength(50, ErrorMessage = "长度不能超过50")]
        public string AdminEmail { get; set; }

        public override ProfileObject GetDefault()
        {
            return new InitializationModel()
            {
                AdminEmail = "admin@cacs.com.cn",
                AdminPassword = "123",
            };
        }
    }
}