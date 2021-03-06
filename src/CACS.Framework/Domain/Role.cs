﻿using CACS.Framework.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CACS.Framework.Domain
{
    public class Role : IdentityRole<int, UserRole>
    {
        public virtual string Description { get; set; }
    }
}
