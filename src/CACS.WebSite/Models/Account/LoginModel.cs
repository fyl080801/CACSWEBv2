using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CACS.WebSite.Models.Account
{
    public class LoginModel
    {
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        public string Username { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual string ReturnUrl { get; set; }

        public bool Remember { get; set; }
    }
}