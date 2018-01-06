﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewCore.HttpClients;

namespace ViewCore.ManagersHttp
{
    public abstract class ManagerHttp
    {

        public string ExcpetionMessage { get; set; }
        public bool IsSuccess { get; set; } = true;

        //TODO There might me problems with the method working asynchronously
        public async Task TryCallApi(Func<Task> action)
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
        }
        }
    }
