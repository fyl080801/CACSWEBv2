using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CACS.WebSite.Models.Account
{
    public class UserListModel
    {
        public string SearchKeyword { get; set; }

        [DisplayName("Email")]
        public string SearchEmail { get; set; }

        [DisplayName("用户名")]
        public string SearchUserName { get; set; }

        [DisplayName("姓名")]
        public string SearchPersonalName { get; set; }
    }
}