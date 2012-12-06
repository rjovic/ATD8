using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ATD8.DataService
{
    public class TodoService
    {
        protected RestClient RestClient;
        public const string TodoUrl = "http://localhost:11332/api/todo/";

        public TodoService()
        {
            RestClient = new RestClient();
        }

        public async Task<List<Todo>> GetAll()
        {
            var serviceResponse = await RestClient.Get(TodoUrl);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<Todo>>(serviceResponse.Content);
                }
                catch (JsonSerializationException ex)
                {
                    Debug.WriteLine(ex.Message + "  " + ex.StackTrace);
                    throw new Exception("Neispravan JSON!");
                }
            }

            throw new Exception("Ne mogu dohvatiti TODO's. \n\nDetalji : " + serviceResponse.Content);
        }

        public async Task<Todo> Create(Todo todo)
        {
            var rawJson = JsonConvert.SerializeObject(todo);
            var serviceResponse = await RestClient.Post(TodoUrl, rawJson);

            if (serviceResponse.StatusCode != HttpStatusCode.Created)
                throw new Exception("Ne mogu dodati novi TODO. \n\nDetalji : " + serviceResponse.Content);

            return JsonConvert.DeserializeObject<Todo>(serviceResponse.Content);
        }

        public async Task Update(Todo todo)
        {
            var rawJson = JsonConvert.SerializeObject(todo);
            var serviceResponse = await RestClient.Put(String.Format(TodoUrl + "{0}", todo.TodoId), rawJson);

            if (serviceResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception("Ne mogu izmjeniti postojeći TODO. \n\nDetalji : " + serviceResponse.Content);
        }

        public async Task Delete(int id)
        {
            var serviceResponse = await RestClient.Delete(String.Format(TodoUrl + "{0}", id));

            if (serviceResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception("Ne mogu izbrisati TODO. \n\nDetalji : " + serviceResponse.Content);
        }
    }
}
