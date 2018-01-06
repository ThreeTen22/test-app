using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Security.Claims;

namespace test_app.Models
{
    [DataContract(Name = "repo")]
    public class Repository
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "html_url")]
        public Uri GitHubHomeUrl { get; set; }

        [DataMember(Name = "homepage")]
        public Uri Homepage { get; set; }

        [DataMember(Name = "watchers")]
        public int Watchers { get; set; }

        [DataMember(Name = "pushed_at")]
        private string JsonDate { get; set; }

        [IgnoreDataMember]
        public DateTime LastPush
        {
            get
            {
                return DateTime.ParseExact(JsonDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            }
        }
    }

    public class WebApiClient
    {
        public static async Task<String> RequestLogicApp(HttpClient client, String uri, ClaimsPrincipal usr)
        {
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            IIdentity id = usr.Identity;
            var dict = new Dictionary<String, String>();
            dict.Add("Auth-Name", id.Name);

            var indx = 0;
            foreach (var i in usr.Claims) {
                dict.Add(i.Type, i.Value);
                indx += 1;
            }

            var content = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/json");
            //var content = JsonConvert.SerializeObject(dict);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write(content);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            var streamTask = client.PostAsync(uri, content);
            var str = await streamTask;
            var body = await str.Content.ReadAsStringAsync();
            str.Dispose();
            streamTask.Dispose();
            client.Dispose();
            return body;

        }
        public static async Task<List<Repository>> ProcessRepositories(HttpClient client, DataContractJsonSerializer serializer)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");

            var msgObj = serializer.ReadObject(await streamTask) as List<Repository>;
            return msgObj;
        }
    }
}