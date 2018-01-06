using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.HttpClients;

namespace ViewCore.ManagersHttp
{
    public abstract class ManagerHttp
    {

        public string ExcpetionMessage { get; set; }
        public bool IsSuccess { get; set; } = true;

        public async Task<bool> TryCallApi(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (CustomHttpRequestException e)
            {
                IsSuccess = false;
                ExcpetionMessage = e.Message;
                throw;
            }
            return IsSuccess;
        }
    }
}
