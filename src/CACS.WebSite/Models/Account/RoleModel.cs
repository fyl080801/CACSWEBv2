﻿using CACS.Framework.Domain;
using CACS.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CACS.WebSite.Models.Account
{
    public class RoleModel : BaseEntityModel<int>
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public static RoleModel Prepare(Role domain)
        {
            return new RoleModel()
            {
                Description = domain.Description,
                Id = domain.Id,
                Name = domain.Name
            };
        }

        public Role ToDomain()
        {
            return new Role()
            {
                Id = this.Id,
                Description = this.Description,
                Name = this.Name
            };
        }
    }
}