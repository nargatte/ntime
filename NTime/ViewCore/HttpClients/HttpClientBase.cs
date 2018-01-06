using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.HttpClients
{
    public abstract class HttpClientBase
    {
        private JsonMediaTypeFormatter _formatter; //Content-Type
        private AccountInfo _accountInfo;
        private ConnectionInfo _connectionInfo;
        private HttpClient _client;
        public string DirectPath { get; set; }

        protected HttpClientBase(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
        {
            _formatter = new JsonMediaTypeFormatter();
            _accountInfo = accountInfo;
            _connectionInfo = connectionInfo;
            _client = new HttpClient();
            SetBaseAddress(controllerName);
            SetAuthenticationData();
        }

        private void SetBaseAddress(string controllerName)
        {
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                _client.BaseAddress = new Uri($"{_connectionInfo.ServerURL}");
            }
            else
            {
                _client.BaseAddress = new Uri($"{_connectionInfo.ServerURL}/{ controllerName}");
            }
        }

        private void SetAuthenticationData()
        {
            if (!string.IsNullOrWhiteSpace(_accountInfo.Token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accountInfo.Token);
            }
        }

        //TODO check the string URI for the simple methods
        protected async Task<TResponse> GetAsync<TResponse>(string uri)
        {
            return await ResolveResponseAsync<TResponse>(await _client.GetAsync(uri));
        }

        protected async Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest request)
        {
            return await ResolveResponseAsync<TResponse>(await _client.PutAsync(uri, request, _formatter));
        }

        protected async Task PutAsync<TRequest>(string uri, TRequest request)
        {
            await ResolveResponseAsync(await _client.PutAsync(uri, request, _formatter));
            return;
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request)
        {
            return await ResolveResponseAsync<TResponse>(await _client.PostAsync(uri, request, _formatter));
        }

        protected async Task PostAsync<TRequest>(string uri, TRequest request)
        {
            await ResolveResponseAsync(await _client.PostAsync(uri, request, _formatter));
            return;
        }


        protected async Task<TResponse> DeleteAsync<TRequest, TResponse>(string uri)
        {
            return await ResolveResponseAsync<TResponse>(await _client.DeleteAsync(string.Empty));
        }

        protected async Task DeleteAsync(string uri)
        {
            await ResolveResponseAsync(await _client.DeleteAsync(string.Empty));
            return;
        }

        private async Task<TResponse> ResolveResponseAsync<TResponse>(HttpResponseMessage response)
        {
            await VerifyStatusCodeAsync(response);
            return await response.Content.ReadAsAsync<TResponse>(new[] { _formatter });
        }

        private async Task ResolveResponseAsync(HttpResponseMessage response)
        {
            await VerifyStatusCodeAsync(response);
            return;
        }

        private async Task VerifyStatusCodeAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomHttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }
    }


}
