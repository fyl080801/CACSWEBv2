using CACS.Framework.Domain;
using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CACS.WebSite.Models.Account
{
    public class UserInfo : BaseEntityModel<string>
    {
        string[] _permissions;

        public string Username { get; set; }

        public string PersonalName { get; set; }

        public int UserType { get; set; }

        public string UserGuid { get; set; }

        public bool Remembered { get; set; }

        public bool Vaild { get; set; }

        public string[] Permissions
        {
            get { return _permissions ?? (_permissions = new string[0]); }
            set { _permissions = value; }
        }

        public static UserInfo Prepare(User domain)
        {
            return new UserInfo()
            {
                Id = domain.Id,
                Username = domain.UserName,
                PersonalName = domain.PersonalName
            };
        }

        public User ToDomain()
        {
            return new User()
            {
                Id = this.Id,
                UserName = this.Username
            };
        }
    }
}