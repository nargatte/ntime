using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Dtos
{
    public class TokenInfoDto
    {
        public TokenInfoDto()
        {

        }
        public TokenInfoDto(string access_token, string token_type, string expires_in, string userName)
        {
            this.access_token = access_token;
            this.token_type = token_type;
            this.expires_in = expires_in;
            this.userName = userName;
        }

        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }


    }
}