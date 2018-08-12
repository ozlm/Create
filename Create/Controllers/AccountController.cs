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
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        connect cn = new connect();

        MongoDatabase m = Create.connect.mongodb;
        IMongoQuery q = Create.connect.query;
        IMongoQuery query_user = Create.connect.query;
        static bool flag = false;

        public AccountController()
        {

        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Users model)
        {
            var collection = m.GetCollection<Users>("Users");
            q = Query<Users>.Where(s => s.Email == model.Email && s.Password == model.Password);
            var model1 = collection.FindOne(q);
            if (model1 != null)//Model gecerli ise
            {
                if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Email), }, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);
                    Session["uid"] = model1._id;
                    Session["UserName"] = model1.UserName;
                    Session["Email"] = model1.Email;
                    identity.AddClaim(new Claim(ClaimTypes.Role, "guest"));
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, "A Person"));
                    var context = HttpContext.GetOwinContext();
                    var authentication = context.Authentication;
                    authentication.SignIn(identity);
                    //if (model.Email == "admin@gmail.com" && model.Password == "admin")
                    //{
                    //    flag = true;
                    //    return RedirectToAction("Admin");
                    //}
                    //else
                    //{
                        return RedirectToAction("Profile");
                    //}
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult Logout()
        {
            var context = HttpContext.GetOwinContext();
            context.Authentication.SignOut();
            return RedirectToAction("Login");
        }


        public ActionResult Admin()
        {
            var collection = m.GetCollection<Users>("Users");
            return View(collection.FindAll().ToList<Users>());
        }


        //admin panelinde kullanıcı işlemleri
        public ActionResult Delete(string Id)
        {
            var collection = m.GetCollection<Users>("Users");
            q = Query<Users>.Where(s => s._id == ObjectId.Parse(Id));
            var model = collection.FindOne(q);
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(Users model)
        {
            var collection = m.GetCollection<Users>("Users");
            collection.Remove(q);
            return RedirectToAction("Admin");
        }

        public ActionResult Edit(string mail, string password)
        {
            var collection = m.GetCollection<Users>("Users");
            q = Query<Users>.Where(s => s.Email == mail && s.Password == password);
            var model = collection.FindOne(q);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Users model)
        {
            var collection = m.GetCollection<Users>("Users");
            var update = Update.Set("UserName", model.UserName).Set("Email", model.Email).Set("Password", model.Password);
            collection.FindAndModify(q, SortBy.Null, update);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Search()
        {
            var collection = m.GetCollection<Users>("Users");
            return View(collection.Find(q).ToList<Users>());
        }


        public ActionResult Settings()
        {
            var user_id = Session["uid"];
            var collection = m.GetCollection<Users>("Users");
            query_user = Query<Users>.Where(s => s._id == (MongoDB.Bson.ObjectId)user_id);
            var model = collection.FindOne(query_user);
            return View(model);
        }
        [HttpPost]
        public ActionResult Settings(Users model)
        {
            var collection = m.GetCollection<Users>("Users");
            var update = Update.Set("UserName", model.UserName).Set("Email", model.Email).Set("Password", model.Password);
            collection.FindAndModify(query_user, SortBy.Null, update);
            return RedirectToAction("Profile");
        }

    }
}

