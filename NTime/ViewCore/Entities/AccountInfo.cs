using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class AccountInfo
    {
        public AccountInfo(string token, string userName)
        {
            Token = token;
            UserName = userName;
        }
        

        #region Properties
        private string _token;
        public string Token
        {
            get { return _token; }
            private set { _token = value; }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            private set { _userName = value; }
        }
        #endregion

    }
}
