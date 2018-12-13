using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogApplication.Models
{
    public class JWTAuthenticationToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public User User { get; set; }
    }
}