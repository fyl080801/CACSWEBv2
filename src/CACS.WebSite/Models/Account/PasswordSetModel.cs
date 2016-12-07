using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CACS.WebSite.Models.Account
{
    public class PasswordSetModel : BaseEntityModel<int>
    {
        [Required]
        public string Password { get; set; }
    }
}