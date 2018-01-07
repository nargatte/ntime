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
        protected HttpClient _client;
        public string DirectPath { get; set; }

        protected HttpClientBase(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
        {
            _formatter = new JsonMediaTypeFormatter();
            _accountInfo = accountInfo;
            _connectionInfo = connectionInfo;
            _client = new HttpClient();
            SetAuthenticationData(_accountInfo);
            SetBaseAddress(controllerName);
        }

        public void SetAuthenticationData(AccountInfo accountInfo)
        {
            if (!string.IsNullOrWhiteSpace(accountInfo.Token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accountInfo.Token);
            }
        }

        private void SetBaseAddress(string controllerName)
        {
            if (string.IsNullOrWhiteSpace(_connectionInfo.ServerURL))
            {
                throw new HttpRequestException("Server URL cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                _client.BaseAddress = new Uri($"{_connectionInfo.ServerURL}");
            }
            else
            {
                _client.BaseAddress = new Uri($"{_connectionInfo.ServerURL}/{ controllerName}");
            }
        }

        //TODO check the string URI for the simple methods
        protected async Task<TResponse> GetAsync<TResponse>(string uri)
        {
            return await ResolveResponseAsync<TResponse>(await _client.GetAsync(uri));
        }

        protected async Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest content)
        {
            return await ResolveResponseAsync<TResponse>(await _client.PutAsync(uri, content, _formatter));
        }

        protected async Task PutAsync<TRequest>(string uri, TRequest content)
        {
            await ResolveResponseAsync(await _client.PutAsync(uri, content, _formatter));
        }

        protected async Task<TResponse> PostUrlEncodedAsync<TResponse>(string uri, IList<KeyValuePair<string,string>> content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(content)};

            //var requestContent = string.Format("site={0}&content={1}", Uri.EscapeDataString("http://www.google.com"),
            //    Uri.EscapeDataString("This is some content"));
            //request.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            var createdUri = request.RequestUri;
            var createdContent = request.Content;
            var createdHeaders = request.Headers;


            return await ResolveResponseAsync<TResponse>(await _client.SendAsync(request));
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest content)
        {
            return await ResolveResponseAsync<TResponse>(await _client.PostAsync(uri, content, _formatter));
        }

        protected async Task PostAsync<TRequest>(string uri, TRequest content)
        {
            await ResolveResponseAsync(await _client.PostAsync(uri, content, _formatter));
        }

        protected async Task PostAsync(string uri)
        {
            await ResolveResponseAsync(await _client.PostAsync(uri, null));
        }



        protected async Task<TResponse> DeleteAsync<TRequest, TResponse>(string uri)
        {
            return await ResolveResponseAsync<TResponse>(await _client.DeleteAsync(string.Empty));
        }

        protected async Task DeleteAsync(string uri)
        {
            await ResolveResponseAsync(await _client.DeleteAsync(string.Empty));
        }

        private async Task<TResponse> ResolveResponseAsync<TResponse>(HttpResponseMessage response)
        {
            await VerifyStatusCodeAsync(response);
            return await response.Content.ReadAsAsync<TResponse>(new[] { _formatter });
        }

        private async Task ResolveResponseAsync(HttpResponseMessage response)
        {
            await VerifyStatusCodeAsync(response);
        }

        private async Task VerifyStatusCodeAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var message = $"{Environment.NewLine}StatusCode: { response.StatusCode} {Environment.NewLine}" +
                                $"Reason: {response.ReasonPhrase} {Environment.NewLine}" +
                                $"Content: {Environment.NewLine}" +
                                $"{await response.Content.ReadAsStringAsync()}";
                throw new CustomHttpRequestException(message);
            }
        }
    }


}
