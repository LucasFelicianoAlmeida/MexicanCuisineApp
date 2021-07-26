using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MexicanCuisine.Service
{
    public static class ApiService<I> where I : class
    {
        private static HttpClient client;

        public static HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new HttpClient();
                    client.BaseAddress = new Uri("http://10.0.2.2:5000/api/");
                }
                return client;
            }
        }


        public static async Task<List<I>> Get(string url)
        {
            try
            {

                var response = await Client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<I[]>(content);

                return json.ToList();

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex?.Message);
                return null;
            }

        }

    }
}
