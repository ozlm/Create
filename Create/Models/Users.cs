using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Create.Models
{
    public class Users
    {
        public ObjectId _id { get; set; }
        public string UserName { get; set; }       
        public string Email { get; set; }
        public string Password { get; set; }
    }
}