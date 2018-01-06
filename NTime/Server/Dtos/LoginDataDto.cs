using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Dtos
{
    public class LoginDataDto
    {
        public string grant_type { get; private set; }
        public string username { get; private set; }
        public string password { get; private set; }

        public LoginDataDto(string userName, string password, string grantType = "password")
        {
            this.username = userName;
            this.password = password;
        }
    }
}