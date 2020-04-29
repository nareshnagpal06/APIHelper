namespace APIHelper
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Identity.Client;
    using Newtonsoft.Json;

    /// <summary>
    /// Resubmission API Helper class
    /// </summary>
    public class ApiHelper : IApiHelper
    {
        /// <summary>
        /// Azure Tenant Id
        /// </summary>
        private string tenantId = ConfigurationManager.AppSettings["AadTenantId"];

        /// <summary>
        /// Azure AD client Id
        /// </summary>
        private string clientId = ConfigurationManager.AppSettings["AadClientId"];

        /// <summary>
        /// Azure AD client Secret
        /// </summary>
        private string clientSecret = ConfigurationManager.AppSettings["AadClientSecret"];

        /// <summary>
        /// Azure Resource Id
        /// </summary>
        private string resourceId = ConfigurationManager.AppSettings["AadResourceId"];

        /// <summary>
        /// AAD Enabled
        /// </summary>
        private bool authEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiHelper"/> class.
        /// </summary>
        /// <param name="authEnabled">Whether AAD auth is enabled</param>
        public ApiHelper(bool authEnabled)
        {
            this.authEnabled = authEnabled;
        }

        /// <summary>
        /// Get request for API
        /// </summary>
        /// <typeparam name="T">Type for deserialization</typeparam>
        /// <param name="requestEndpoint">URI for service call</param>
        /// <param name="additionalHeaders">Optional Header collection</param>
        /// <returns>deserialized object </returns>
        public async Task<T> GetRequestAsync<T>(string requestEndpoint, Dictionary<string, string> additionalHeaders)
        {
            using (var client = new HttpClient())
            {
                if (this.authEnabled)
                {
                    // Add Access token to headers
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.GetAccessToken().Result.AccessToken);
                }

                // Add additional headers
                AddHeaders(client, additionalHeaders);

                var httpResponseMessage = await client.GetAsync(requestEndpoint);
                var response = await httpResponseMessage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(response, new JsonSerializerSettings() { Formatting = Formatting.None });
            }
        }

        /// <summary>
        /// Get request for API
        /// </summary>
        /// <typeparam name="T">Type for deserialization</typeparam>
        /// <param name="requestEndpoint">URI for service call</param>
        /// <returns>deserialized object </returns>
        public async Task<T> GetRequestAsync<T>(string requestEndpoint)
        {
            return await this.GetRequestAsync<T>(requestEndpoint, null);
        }

        /// <summary>
        /// Post request for API with headers
        /// </summary>
        /// <typeparam name="TRequestType">RequestType Type for deserialization</typeparam>
        /// <typeparam name="TResponseType">ResponseType Type for deserialization</typeparam>
        /// <param name="requestEndpoint">URI for service call</param>
        /// <param name="requestBody">request body to be posted</param>
        /// <param name="additionalHeaders">Optional Header collection</param>
        /// <returns>deserialized object </returns>
        public async Task<TResponseType> PostRequestAsync<TRequestType, TResponseType>(string requestEndpoint, TRequestType requestBody, Dictionary<string, string> additionalHeaders)
        {
            using (var client = new HttpClient())
            {
                if (this.authEnabled)
                {
                    // Add Access token to headers
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.GetAccessToken().Result.AccessToken);
                }

                // Add additional headers
                AddHeaders(client, additionalHeaders);

                var json = JsonConvert.SerializeObject(requestBody);
                var httpContent = new StringContent(json);

                var httpResponseMessage = await client.PostAsync(requestEndpoint, httpContent);
                var response = await httpResponseMessage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResponseType>(response);
            }
        }

        /// <summary>
        /// Post request for API without headers
        /// </summary>
        /// <typeparam name="TRequestType">RequestType Type for deserialization</typeparam>
        /// <typeparam name="TResponseType">ResponseType Type for deserialization</typeparam>
        /// <param name="requestEndpoint">URI for service call</param>
        /// <param name="requestBody">request body to be posted</param>
        /// <returns>deserialized object </returns>
        public async Task<TResponseType> PostRequestAsync<TRequestType, TResponseType>(string requestEndpoint, TRequestType requestBody)
        {
            return await this.PostRequestAsync<TRequestType, TResponseType>(requestEndpoint, requestBody, null);
        }

        /// <summary>
        /// Put request for API
        /// </summary>
        /// <typeparam name="TRequest">Request Type for deserialization</typeparam>
        /// <typeparam name="TResponse">Response Type for deserialization</typeparam>
        /// <param name="requestEndpoint">URI for service call</param>
        /// <param name="requestBody">request body to be posted</param>
        /// <param name="additionalHeaders">Optional Header collection</param>
        /// <returns>deserialized object </returns>
        public async Task<TResponse> PutRequestAsync<TRequest, TResponse>(string requestEndpoint, TRequest requestBody, Dictionary<string, string> additionalHeaders)
        {
            using (var client = new HttpClient())
            {
                if (this.authEnabled)
                {
                    // Add Access token to headers
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.GetAccessToken().Result.AccessToken);
                }

                // Add additional headers
                AddHeaders(client, additionalHeaders);

                var json = JsonConvert.SerializeObject(requestBody);
                var httpContent = new StringContent(json);

                var httpResponseMessage = await client.PutAsync(requestEndpoint, httpContent);
                var response = await httpResponseMessage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResponse>(response);
            }
        }

        /// <summary>
        /// Put request for API
        /// </summary>
        /// <typeparam name="TRequest">Request Type for deserialization</typeparam>
        /// <typeparam name="TResponse">Response Type for deserialization</typeparam>
        /// <param name="requestEndpoint">URI for service call</param>
        /// <param name="requestBody">request body to be posted</param>
        /// <returns>deserialized object </returns>
        public async Task<TResponse> PutRequestAsync<TRequest, TResponse>(string requestEndpoint, TRequest requestBody)
        {
            return await this.PutRequestAsync<TRequest, TResponse>(requestEndpoint, requestBody, null);
        }

        /// <summary>
        /// Delete request for API
        /// </summary>
        /// <typeparam name="T">Type for deserialization</typeparam>
        /// <param name="requestEndpoint">URI for service call</param>
        /// <param name="additionalHeaders">Optional Header collection</param>
        /// <returns>deserialized object </returns>
        public async Task<T> DeleteRequestAsync<T>(string requestEndpoint, Dictionary<string, string> additionalHeaders)
        {
            using (var client = new HttpClient())
            {
                if (this.authEnabled)
                {
                    // Add Access token to headers
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.GetAccessToken().Result.AccessToken);
                }

                // Add additional headers
                AddHeaders(client, additionalHeaders);

                var httpResponseMessage = await client.DeleteAsync(requestEndpoint);
                var response = await httpResponseMessage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(response, new JsonSerializerSettings() { Formatting = Formatting.None });
            }
        }

        /// <summary>
        /// Delete request for API
        /// </summary>
        /// <typeparam name="T">Type for deserialization</typeparam>
        /// <param name="requestEndpoint">URI for service call</param>
        /// <returns>deserialized object </returns>
        public async Task<T> DeleteRequestAsync<T>(string requestEndpoint)
        {
            return await this.DeleteRequestAsync<T>(requestEndpoint, null);
        }

        private static void AddHeaders(HttpClient httpClient, Dictionary<string, string> additionalHeaders)
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            if (additionalHeaders == null)
            {
                return;
            }

            foreach (KeyValuePair<string, string> current in additionalHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(current.Key, current.Value);
            }
        }

        private async Task<AuthenticationResult> GetAccessToken()
        {
            var app = ConfidentialClientApplicationBuilder.Create(this.clientId)
               .WithAuthority(AzureCloudInstance.AzurePublic, this.tenantId)
               .WithClientSecret(this.clientSecret)
               .Build();
            var scopes = new[] { this.resourceId + "/.default" };
            AuthenticationResult authenticationResult = await app.AcquireTokenForClient(scopes)
                       .ExecuteAsync();
            return authenticationResult;
        }

    }
}
