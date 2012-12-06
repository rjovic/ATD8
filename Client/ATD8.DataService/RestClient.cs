using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ATD8.DataService
{
    public class RestClient
    {
        public async Task<RestClientResponse> Get(string url, string data = null)
        {
            return await Request(url, HttpMethod.Get, data);
        }

        public async Task<RestClientResponse> Post(string url, string data = null)
        {
            return await Request(url, HttpMethod.Post, data);
        }

        public async Task<RestClientResponse> Put(string url, string data = null)
        {
            return await Request(url, HttpMethod.Put, data);
        }

        public async Task<RestClientResponse> Delete(string url, string data = null)
        {
            return await Request(url, HttpMethod.Delete, data);
        }

        public async Task<RestClientResponse> Request(string url, HttpMethod method, string data = null)
        {
            var http = new HttpClient();
            var msg = new HttpRequestMessage(method, url);
            msg.Headers.Add("Accept", "application/json");

            if (data != null)
            {
                msg.Content = new StringContent(data, Encoding.UTF8, "application/json");
            }

            try
            {
                var response = await http.SendAsync(msg);

                var result = new RestClientResponse
                {
                    Content = await response.Content.ReadAsStringAsync(),
                    StatusCode = response.StatusCode
                };

                return result;
            }
            catch (Exception)
            {
                throw new HttpRequestException("Ne mogu dohvatiti podatke. Provjerite da li ste spojeni na internet.");
            }
        }
    }

    public class RestClientResponse
    {
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
