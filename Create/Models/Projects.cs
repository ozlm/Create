using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Create.Models
{
    public class Projects
    {
        public ObjectId _id { get; set; }
        public String projectName { get; set; }
        public String projectSubject { get; set; }
        public DateTime projectEndDate { get; set; }
        public DateTime projectStartDate { get; set; }
     }
}