using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PouleSimulator
{
    public class RandomPlayerNameFactory
    {
        [Serializable]
        private struct RandomNameRequest
        {
            [JsonProperty("results")]
            public RandomPerson[] Results { get; }
            [JsonProperty("info")]
            public RandomNameInfo Info { get; }

            [JsonConstructor]
            public RandomNameRequest(RandomPerson[] results, RandomNameInfo info) {
                this.Results = results;
                this.Info = info;
            }
        }

        [Serializable]
        private struct RandomPerson
        {
            [JsonProperty("name")]
            public RandomName Name { get; }

            [JsonConstructor]
            public RandomPerson(RandomName name) {
                this.Name = name;
            }
        }

        [Serializable]
        public struct RandomName
        {
            [JsonProperty("first")]
            public string FirstName;
            [JsonProperty("last")]
            public string LastName;

            [JsonConstructor]
            public RandomName(string first, string last) {
                this.FirstName = first;
                this.LastName = last;
            }

            public static implicit operator string(RandomName name) => $"{name.FirstName} {name.LastName}";
        }

        [Serializable]
        private struct RandomNameInfo
        {
            [JsonProperty("seed")]
            public string Seed;
            [JsonProperty("results")]
            public int ResultCount;
            [JsonProperty("page")]
            public int Page;
            [JsonProperty("version")]
            public string Version;

            [JsonConstructor]
            public RandomNameInfo(string seed, int results, int page, string version) {
                this.Seed = seed;
                this.ResultCount = results;
                this.Page = page;
                this.Version = version;
            }
        }

        private readonly Random random;

        public RandomPlayerNameFactory(Random random = null) {
            this.random = random ?? new Random();
        }

        public async Task<RandomName[]> CreateRandomPlayerNames(int count) {
            try {
                return await GetRandomNamesFromWeb(count);
            } catch {
                Console.WriteLine("[Warning] Failed to retrieve random names from the web, falling back to locally stored list of names.");
                return GetRandomNamesFromDisk(count);
            }
        }

        private async Task<RandomName[]> GetRandomNamesFromWeb(int count) {
            HttpWebRequest request = WebRequest.CreateHttp($"https://randomuser.me/api/?gender=male&nat=us,nl,gb,fr,es,de&inc=name&results={count}&seed={random.Next()}");
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "GET";
            request.Timeout = 2000;

            using(HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync())) {
                using(StreamReader reader = new StreamReader(response.GetResponseStream())) {
                    string responseString = await reader.ReadToEndAsync();
                    RandomNameRequest randomNames = JsonConvert.DeserializeObject<RandomNameRequest>(responseString);
                    return randomNames.Results.Select(person => person.Name).ToArray();
                }
            }
        }

        private RandomName[] GetRandomNamesFromDisk(int count) {
            RandomName[] randomNames = new RandomName[count];
            for(int i = 0; i < count; i++) { randomNames[i] = new RandomName { FirstName = "Mock", LastName = "Data" }; }
            return randomNames;
        }
    }
}
