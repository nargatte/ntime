﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore
{
    public class AccountInfo
    {
        public AccountInfo()
        {

        }
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
            set { _token = value; }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        #endregion
        public bool IsAuthenticated => !String.IsNullOrWhiteSpace(Token);
    }
}
