using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Create.Models
{
    public class ProjectUser
    {
        public ObjectId _id { get; set; }
        public ObjectId userID { get; set; }
        public ObjectId projectID { get; set; }
    }
}