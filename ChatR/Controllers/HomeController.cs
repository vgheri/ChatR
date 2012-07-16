using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatR.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("username", "Username is required");
                return View();
            }
            return RedirectToAction("Chat", "Home", new { username = username });
        }

        public ActionResult Chat(string username)
        {
            ViewBag.username = username;
            return View();
        }

    }
}
