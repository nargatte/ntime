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
        private string _controllerName;

        protected HttpClientBase(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
        {
            _formatter = new JsonMediaTypeFormatter();
            _accountInfo = accountInfo;
            _connectionInfo = connectionInfo;
            _client = new HttpClient();
            SetAuthenticationData(_accountInfo);
            _controllerName = controllerName;
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
                _client.BaseAddress = new Uri($"{_connectionInfo.ServerURL}/");
            }
            else
            {
                _client.BaseAddress = new Uri($"{_connectionInfo.ServerURL}/api/");
                _controllerName = controllerName;
            }
        }

        protected string GetPageQuery(int itemsOnPage, int pageNumber, bool addStartQuerySign = true)
        {
            var result = "";
            if (addStartQuerySign)
            {
                result += "?";
            }
            else
            {
                result += "&";
            }
            return result + $"ItemsOnPage={itemsOnPage}&PageNumber={pageNumber}";
        }

        private string AddControllerName(string uri, bool addSlash = true)
        {
            string resultUri = "";
            if(!string.IsNullOrWhiteSpace(_controllerName))
            {
                resultUri += _controllerName;
                if (addSlash)
                {
                    resultUri += "/";
                }
            }
            resultUri += uri;
            return resultUri;
        }

        //TODO check the string URI for the simple methods
        protected async Task<TResponse> GetAsync<TResponse>(string uri, bool addSlash = true)
        {
            return await ResolveResponseAsync<TResponse>(await _client.GetAsync(AddControllerName(uri, addSlash)));
        }

        //protected async Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest content)
        //{
        //    return await ResolveResponseAsync<TResponse>(await _client.PutAsync(uri, content, _formatter));
        //}

        protected async Task PutAsync<TRequest>(string uri, TRequest content)
        {
            await ResolveResponseAsync(await _client.PutAsync(AddControllerName(uri), content, _formatter));
        }

        protected async Task<TResponse> PostUrlEncodedAsync<TResponse>(string uri, IList<KeyValuePair<string, string>> content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, AddControllerName(uri)) { Content = new FormUrlEncodedContent(content) };
            return await ResolveResponseAsync<TResponse>(await _client.SendAsync(request));
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest content, bool addSlash = true)
        {
            return await ResolveResponseAsync<TResponse>(await _client.PostAsync(AddControllerName(uri, addSlash), content, _formatter));
        }

        protected async Task PostAsync<TRequest>(string uri, TRequest content)
        {
            await ResolveResponseAsync(await _client.PostAsync(AddControllerName(uri), content, _formatter));
        }

        protected async Task PostAsync(string uri)
        {
            await ResolveResponseAsync(await _client.PostAsync(AddControllerName(uri), null));
        }

        //protected async Task<TResponse> DeleteAsync<TRequest, TResponse>(string uri)
        //{
        //    return await ResolveResponseAsync<TResponse>(await _client.DeleteAsync(string.Empty));
        //}

        protected async Task DeleteAsync(string uri)
        {
            await ResolveResponseAsync(await _client.DeleteAsync(AddControllerName(uri)));
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
                                $"URI: {response.RequestMessage.RequestUri.ToString() ?? "Server could not be found"} {Environment.NewLine}" +
                                $"Content: {Environment.NewLine}" +
                                $"{await response.Content.ReadAsStringAsync()}";
                throw new CustomHttpRequestException(message);
            }
        }
    }


}
