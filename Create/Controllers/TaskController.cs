using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using Create.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Create.Controllers
{
    public class TaskController : Controller
    {
        connect cn = new connect();
        MongoDatabase m = Create.connect.mongodb;
        IMongoQuery q = Create.connect.query;
        static String a;
        
        // GET: /Task/

        public ActionResult DeleteTask(string Id)
        {
            var collection = m.GetCollection<Tasks>("Tasks");
            q = Query<Tasks>.Where(s => s._id == ObjectId.Parse(Id));
            var model = collection.FindOne(q);
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteTask(Tasks model)
        {
            var collection = m.GetCollection<Tasks>("Tasks");
            collection.Remove(q);
            return RedirectToAction("TaskListPro","Task");
        }

        public ActionResult EditTask(string Id)
        {
            var collection = m.GetCollection<Tasks>("Tasks");
            q = Query<Projects>.Where(s => s._id == ObjectId.Parse(Id));
            var model = collection.FindOne(q);
            return View(model);
        }
       [HttpPost]
        public ActionResult EditTask(Tasks model)
        {
            var collection = m.GetCollection<Tasks>("Tasks");
            var update = Update.Set("taskName", model.taskName).Set("taskStartDate", model.taskStartDate).Set("taskEndDate", model.taskEndDate);
            collection.FindAndModify(q, SortBy.Null, update);
            return RedirectToAction("TaskListPro", "Task");
        }

        public ActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTask(Tasks model)
        {
            var collection = m.GetCollection<Tasks>("Tasks");
            collection.Insert(model);
            return RedirectToAction("");
        }

        public ActionResult TaskMember()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TaskMember(Tasks model, Users us)
        {
            var collec = m.GetCollection<BsonDocument>("TaskUserPro");
            var collection = m.GetCollection<BsonDocument>("Tasks");
            var collection_user = m.GetCollection<Users>("Users");
            String email = Request.Form["txtuser"];
            String taskName = Request.Form["txttaskName"];
            IMongoQuery query_user = Query<Users>.Where(s => s.Email == email);
            IMongoQuery query_task = Query<Tasks>.Where(s => s.taskName == taskName);
            var model1 = collection_user.FindOne(q);
            var model2 = collection.FindOne(query_task);
            if (model1 != null && model2 != null)
            {
                var document = new BsonDocument{
               {"taskName",taskName},
               {"userID", model1._id}
           
           };
                collec.Insert(document);
            }

            return RedirectToAction("Edit", "Account");
        }

        //projeye task ekleme
        public ActionResult TaskProje(String Id)
        {
            a=Id;
            return View();
        }

        [HttpPost]
        public ActionResult TaskProje(Tasks model)
        {           
           var collection = m.GetCollection<BsonDocument>("Tasks");
            var collection_project = m.GetCollection<Projects>("Projects");
          
            var document = new BsonDocument{            
                {"taskName",model.taskName},
                {"taskEndDate",model.taskEndDate},
                {"taskStartDate",model.taskStartDate},
                {"projectID",a}                
             };
            collection.Insert(document);
               return RedirectToAction("Profile", "Account");
        }

        //admin panelde ve user girisinde kullanıcının projelerini ve bağlı olduğu taskları gösterir
        public ActionResult TaskProUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TaskProUser(Tasks model)
        {
            var collec = m.GetCollection<BsonDocument>("TaskUserPro");
            var collection = m.GetCollection<BsonDocument>("Tasks");
            var collection_project = m.GetCollection<Projects>("Projects");
            var collection_user = m.GetCollection<Users>("Users");
            String projectName = Request.Form["txtprojectName"];
            String taskName = Request.Form["txttaskName"];
            String email = Request.Form["txtemail"];
            IMongoQuery query_project = Query<Projects>.Where(s => s.projectName == projectName);
            IMongoQuery query_task = Query<Tasks>.Where(s => s.taskName == taskName);
            IMongoQuery query_user = Query<Users>.Where(s => s.Email == email);
            var model1 = collection_project.FindOne(query_project);
            var model2 = collection.FindOne(query_task);
            var model3 = collection_user.FindOne(query_user);
            if (model1 != null && model2 != null && model3 != null)
            {
                var document = new BsonDocument{
              {"projectName",projectName},
                {"taskName",taskName},
                {"email",email}
                 };
                collec.Insert(document);
            }
            return RedirectToAction("Edit", "Account");
        }
        

        //admin panelde projects içinde o projeye ait taskları gösterir
        public ActionResult TaskListPro(string ID)
        {
            var collection_user = m.GetCollection<Projects>("Projects");
            IMongoQuery query = Query<Projects>.Where(s => s._id == ObjectId.Parse(ID));
            Projects model_proje = collection_user.FindOne(query);

             var collection_task = m.GetCollection<Tasks>("Tasks");
             IMongoQuery query_task = Query<Tasks>.Where(t => t.projectID == model_proje._id);
             Tasks[] model_task = collection_task.Find(query_task).ToArray();
                        
            return View(model_task);
        }


        public ActionResult ListTask(string ID)
        {
            
            var collection_user = m.GetCollection<Projects>("Projects");
            IMongoQuery query = Query<Projects>.Where(s => s._id == ObjectId.Parse(ID));
            Projects model_proje = collection_user.FindOne(query);

            var collection_task = m.GetCollection<Tasks>("Tasks");
            IMongoQuery query_task = Query<Tasks>.Where(t => t.projectID == model_proje._id);
            Tasks[] model_task = collection_task.Find(query_task).ToArray();

            return View(model_task);

        }
       
    }
}
