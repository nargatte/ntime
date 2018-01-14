using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewCore.HttpClients;

namespace ViewCore.ManagersHttp
{
    public abstract class ManagerHttp
    {
        protected AccountInfo _accountInfo;
        protected ConnectionInfo _connectionInfo;

        public string ExcpetionMessage { get; set; }
        public bool IsSuccess { get; set; } = true;
        

        protected ManagerHttp(AccountInfo accountInfo, ConnectionInfo connectionInfo)
        {
            _accountInfo = accountInfo;
            _connectionInfo = connectionInfo;
        }

        //TODO There might me problems with the method working asynchronously
        public async Task TryCallApi(Func<Task> action, string successMessage = null)
        {
            IsSuccess = true;
            try
            {
                await action();
            }
            catch (CustomHttpRequestException e)
            {
                IsSuccess = false;
                ExcpetionMessage = e.Message;
            }
            catch (HttpRequestException e)
            {
                IsSuccess = false;
                ExcpetionMessage = $"Niepoprawny adres serwera {Environment.NewLine}{e.Message}";
            }
            if (!IsSuccess)
            {
                MessageBox.Show(ExcpetionMessage);
            }
            if (IsSuccess && !string.IsNullOrWhiteSpace(successMessage))
            {
                MessageBox.Show(successMessage);
            }
        }
        }
    }
