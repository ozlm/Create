using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using Create.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Create.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        connect cn = new connect();
        MongoDatabase m = Create.connect.mongodb;
        static bool flag = false;

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(Users model)
        {
            var collection = m.GetCollection<Users>("Users");
            collection.Insert(model);
            if (flag == true)
            {
                return RedirectToAction("Admin");
            }
            return RedirectToAction("Profile","Account");
        }

        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(Users model)
        {
            var collection = m.GetCollection<Users>("Users");
            collection.Insert(model);
          
            return RedirectToAction("Admin","Account");

        }

    }
}
