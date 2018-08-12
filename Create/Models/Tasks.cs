using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Create.Models
{
    public class Tasks
    {
        public ObjectId _id { get; set; }
        public String taskName { get; set; }
        public DateTime taskEndDate { get; set; }
        public DateTime taskStartDate { get; set; }
        public ObjectId projectID { get; set; }
    }
}