using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSideTimeR.Models
{
    public class UserCredentials
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ConnectionId { get; set; }
        public int Status { get; set; }
    }
}