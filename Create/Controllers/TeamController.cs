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
    public class TeamController : Controller
    {
        //
        // GET: /Team/

        connect cn = new connect();
        MongoDatabase m = Create.connect.mongodb;
        IMongoQuery q = Create.connect.query;
        static IMongoQuery queryTeam = Create.connect.query;

        //!!takıma user ekle TeamUser'a ekle idler olacak  sadece

        public ActionResult AddFriend()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFriend(Team model, Users us)
        {
            var collectionTeam = m.GetCollection<Team>("Team");
            var collection_user = m.GetCollection<Users>("Users");
            var collection = m.GetCollection<BsonDocument>("TeamUser");
            String email = Request.Form["txtuser"];
            String teamName = Request.Form["txtteamName"];
            IMongoQuery query = Query<Users>.Where(s => s.Email == email);
            IMongoQuery queryTeam = Query<Team>.Where(s => s.teamName == teamName);

            var model1 = collection_user.FindOne(q);
            var modelTeam = collectionTeam.FindOne(queryTeam);
            if (model1 != null && modelTeam != null)
            {
                var document = new BsonDocument{
               {"teamId",modelTeam._id},
               {"userID", model1._id}

           };
                collection.Insert(document);
            }

            return RedirectToAction("TeamList", "Team");
        }
        

        public ActionResult AddTeam()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddTeam(Team model)
        {
            var collection = m.GetCollection<Team>("Team");
            collection.Insert(model);
            return RedirectToAction("TeamList", "Team");
        }


        //takım arkadaslarını listele
        public ActionResult TeamFriend()
        {
            var collecUser = m.GetCollection<Users>("Users");
            var collecTeam = m.GetCollection<Team>("Team");
            var collection = m.GetCollection<TeamUser>("TeamUser");
            var userId = Session["uid"];
            IMongoQuery query = Query<TeamUser>.Where(s => s.userId == (MongoDB.Bson.ObjectId)userId);
            var model = collection.FindOne(query);

            queryTeam = Query<Team>.Where(s => s._id == model.teamId);
            IMongoQuery queryUser = Query<Users>.Where(s => s._id != model.userId);

            if (queryUser != null)
            {
                if (model.userId != (MongoDB.Bson.ObjectId)userId)
                {
                    var modelUser = collecUser.FindOne(queryUser);
                    return View(modelUser);
                }
            }
            var modelTeam = collecTeam.FindOne(queryTeam);
            Session["teamNamee"] = modelTeam.teamName;
            return View(collecUser.FindAll().ToList<Users>());
        }
        [HttpPost]
        public ActionResult TeamFriend(Users model)
        {
            var collecTeam = m.GetCollection<Team>("Team");
            var collection = m.GetCollection<Users>("Users");
            var modelTeam = collecTeam.FindOne(queryTeam);
            Session["teamNamee"] = modelTeam.teamName;
            return View(collection.FindAll().ToList<Users>());
        }
    }
}
