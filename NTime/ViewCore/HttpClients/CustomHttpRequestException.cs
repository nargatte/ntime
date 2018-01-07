using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.HttpClients
{
    public class CustomHttpRequestException : Exception
    {
        public CustomHttpRequestException(string responseMessage) : base("Wrong http request or problems with the server." + responseMessage)
        {
            
        }
    }
}
