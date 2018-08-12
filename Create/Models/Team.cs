using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Create.Models
{
    public class Team
    {
        public ObjectId _id { get; set; }
        public String teamName { get; set; }
        public ObjectId userID { get; set; }
    }
}