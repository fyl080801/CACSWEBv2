using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CACS.WebSite.Models.Account
{
    public class LogListModel
    {
        [DisplayName("关键字")]
        public string SearchKeyword { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }
    }
}