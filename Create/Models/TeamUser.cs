using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Create.Models
{
    public class TeamUser
    {
      public ObjectId _id { get; set; }
      public ObjectId userId { get; set; }
      public ObjectId teamId { get; set; }
    }
}