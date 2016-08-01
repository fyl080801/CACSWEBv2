using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Interfaces;
using CACS.Framework.Mvc.Controllers;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CACS.WebSite.Controllers
{
    public class TestController : CACSController
    {
        ApplicationUserManager _userManager;

        public TestController(HttpContextBase context)
        {
            _userManager = context.GetOwinContext().Get<ApplicationUserManager>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestMethod()
        {
            try
            {
                var db = EngineContext.Current.Resolve<IDbContext>();
                var rep = EngineContext.Current.Resolve<IRepository<EventLog>>();
                var ewp2 = EngineContext.Current.Resolve<IRepository<Message>>();

                using (var trans = db.Transaction.BeginTransaction())
                {
                    try
                    {
                        rep.Insert(new EventLog()
                        {
                            EventName = "aaa",
                            EventTime = DateTime.Now,
                            EventType = EventTypes.Special,
                            UserId = _userManager.FindByName(HttpContext.User.Identity.Name).Id,
                            SourceId = "sss",
                            SourceName = "test"
                        });
                        ewp2.Insert(new Message()
                        {
                            SenderId = _userManager.FindByName(HttpContext.User.Identity.Name).Id,
                            ReceiverId = _userManager.FindByName(HttpContext.User.Identity.Name).Id,
                            SendTime = DateTime.Now,
                            //Title = "aaa"
                        });
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return Json(false);
                    }

                }

                using (db.Transaction.BeginTransaction())
                {

                }
                return Json(true);
            }
            catch
            {
                return Json(false);
            }

            //var acc = EngineContext.Current.Resolve<IAccountService>();
            //var xxx = acc.GetAuthorizes();
            //var aaaa = EngineContext.Current.Resolve<IAccountService>();
            //var auth = aaaa.GetAuthorizeNames();
            //string aid = GAuthId("CACS.WebSite.Areas.Testx.Controllers", "Home", "AAA");

        }
    }
}