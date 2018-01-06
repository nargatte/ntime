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

        public IList<KeyValuePair<string,string>> GetDictionary()
        {
            var dict = new List<KeyValuePair<string, string>>();
            dict.Add(new KeyValuePair<string, string>(nameof(grant_type), grant_type));
            dict.Add(new KeyValuePair<string, string>(nameof(username), username));
            dict.Add(new KeyValuePair<string, string>(nameof(password), password));
            //foreach (var item in this.GetType().GetProperties())
            //{
            //    dict.Add(nameof(item), item.GetValue(item, null).ToString());
            //}
            return dict;
        }
    }
}