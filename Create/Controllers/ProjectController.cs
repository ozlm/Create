using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using Create.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Create.Controllers
{
    public class ProjectController : Controller
    {
        //
        // GET: /Project/
        connect cn = new connect();
        MongoDatabase m = Create.connect.mongodb;
        IMongoQuery q = Create.connect.query;
        static IMongoQuery query = Create.connect.query;
        IMongoQuery query_user = Create.connect.query;
        IMongoQuery query_project = Create.connect.query;
        static String a;

        public ActionResult AddProject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProject(Projects model)
        {
            var collection = m.GetCollection<Projects>("Projects");
            var collec = m.GetCollection<BsonDocument>("ProjectUser");
            var collection_user = m.GetCollection<Users>("Users");
            collection.Insert(model);

            var user_id = Session["uid"];
            IMongoQuery query_user = Query<Users>.Where(s => s._id == (MongoDB.Bson.ObjectId)user_id);
            var model_user = collection_user.FindOne(query_user);
            if (model_user != null)
            {
                var document = new BsonDocument{
               {"projectID",model._id},
               {"userID", (MongoDB.Bson.ObjectId)user_id}           
           };
                collec.Insert(document);
            }

            return RedirectToAction("Projects", "Project");
        }

        public ActionResult DeleteProject(string Id)
        {
            var collection = m.GetCollection<Projects>("Projects");
            q = Query<Projects>.Where(s => s._id == ObjectId.Parse(Id));
            var model = collection.FindOne(q);
            return View(model);
        }
        [HttpPost]
        public ActionResult DeleteProject(Projects model)
        {
            var collection = m.GetCollection<Projects>("Projects");
            collection.Remove(q);
            return RedirectToAction("Projects", "Project");
        }

        public ActionResult EditProject(string Id)
        {
            var collection = m.GetCollection<Projects>("Projects");
            q = Query<Projects>.Where(s => s._id == ObjectId.Parse(Id));
            var model = collection.FindOne(q);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditProject(Projects model)
        {
            var collection = m.GetCollection<Projects>("Projects");
            var update = Update.Set("projectName", model.projectName).Set("projectSubject", model.projectSubject).Set("projectEndDate", model.projectEndDate).Set("projectStartDate", model.projectStartDate);
            collection.FindAndModify(q, SortBy.Null, update);
            return RedirectToAction("Projects", "Project");
        }
        

        //login yapıldıktan sonra kullanıcının projeye arkadaş eklemesi
        public ActionResult ProjectMember(String Id)
        {
            a = Id;
            return View();
        }
        [HttpPost]
        public ActionResult ProjectMember()
        {
            var collec = m.GetCollection<BsonDocument>("ProjectUser");
            var collection = m.GetCollection<BsonDocument>("Projects");
            var collection_user = m.GetCollection<Users>("Users");
            String email = Request.Form["txtuser"];
            //String projectName = Request.Form["txtprojeName"];
            string[] users = email.Split(',');
            for (int i = 0; i < users.Length; i++)
            {
                query_user = Query<Users>.Where(s => s.Email == users[i]);
                query_project = Query<Projects>.Where(s => s._id == ObjectId.Parse(a));
                var model1 = collection_user.FindOne(query_user);
                var model2 = collection.FindOne(query_project);
                if (model1 != null && model2 != null)
                {
                    var document = new BsonDocument{
               {"projectID",a},
               {"userID", model1._id}           
           };
                    collec.Insert(document);
                }
                else
                    Response.Write("<script>alert('asds')</script>");
            }

            return RedirectToAction("Projects", "Project");
        }


        //admin panelde kullanıcıya ait olan projeler
        public ActionResult ProjectList(string ID)
        {
            var collection_user = m.GetCollection<Users>("Users");
            IMongoQuery query = Query<Users>.Where(s => s._id == ObjectId.Parse(ID));
            
            var model_user = collection_user.FindOne(query);
            var collection = m.GetCollection<ProjectUser>("ProjectUser");
            IMongoQuery query_user = Query<ProjectUser>.Where(p => p.userID == model_user._id);
            ProjectUser[] model_pro = collection.Find(query_user).ToArray();

            var collection_pro = m.GetCollection<Projects>("Projects");
            Projects[] model = new Projects[model_pro.Length];
            for (int i = 0; i < model_pro.Length; i++)
            {   
                IMongoQuery query_proj = Query<Projects>.Where(r => r._id == model_pro[i].projectID);
                if (query_proj != null)
                {
                    model[i] = collection_pro.FindOne(query_proj);
                }
            }
            return View(model);           
        }
     
        //kullanıcı girişinde kullanıcıya ait olan projeler       
        public ActionResult Projects()
        {
            var user_id = Session["uid"];
            var collection = m.GetCollection<ProjectUser>("ProjectUser");
            IMongoQuery query_user = Query<ProjectUser>.Where(p => p.userID == (MongoDB.Bson.ObjectId)user_id);
            ProjectUser[] model_pro = collection.Find(query_user).ToArray();
            var collection_pro = m.GetCollection<Projects>("Projects");

            Projects[] model = new Projects[model_pro.Length];
            for (int i = 0; i < model_pro.Length; i++)
            {
                IMongoQuery query_proj = Query<Projects>.Where(r => r._id == model_pro[i].projectID);
                if (query_proj != null)
                {
                    model[i] = collection_pro.FindOne(query_proj);
                }
            }
            return View(model);
        }
             
    }
}
