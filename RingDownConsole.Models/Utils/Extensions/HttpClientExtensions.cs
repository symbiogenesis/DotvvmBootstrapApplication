using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RingDownConsole.Interfaces;
using Newtonsoft.Json;

namespace RingDownConsole.Utils.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<T> GetDataAsync<T>(this HttpClient client, int recordId) where T : class
        {
            return client.GetDataAsync<T>(recordId.ToString());
        }

        public static async Task<T> GetDataAsync<T>(this HttpClient client, string recordId) where T : class
        {
            var requestUri = $"/api/{typeof(T).Name}/{recordId}";

            T data = null;
            var response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<T>(responseString);
            }
            return data;
        }

        public static async Task<IEnumerable<T>> GetDataAsync<T>(this HttpClient client)
        {
            var requestUri = $"/api/{typeof(T).Name}";

            IEnumerable<T> data = null;
            var response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<IEnumerable<T>>(responseString);
            }
            return data;
        }

        public static Task<HttpResponseMessage> PutDataAsync<T>(this HttpClient client, T record) where T : IIdentifiable
        {
            var requestUri = $"/api/{typeof(T).Name}/{record.Id}";

            var serializedRecord = JsonConvert.SerializeObject(record);
            var httpContent = new StringContent(serializedRecord, Encoding.UTF8, "application/json");
            return client.PutAsync(requestUri, httpContent);
        }

        public static Task<HttpResponseMessage> PostDataAsync<T>(this HttpClient client, T newRecord)
        {
            var requestUri = $"/api/{typeof(T).Name}/";

            var data = JsonConvert.SerializeObject(newRecord);
            var httpContent = new StringContent(data, Encoding.UTF8, "application/json");
            return client.PostAsync(requestUri, httpContent);
        }

        public static Task<HttpResponseMessage> DeleteDataAsync<T>(this HttpClient client, T record) where T : IIdentifiable
        {
            var requestUrl = $"/api/{typeof(T).Name}/{record.Id}";

            return client.DeleteAsync(requestUrl);
        }
    }

    public static class LocationExtensions
    {
        public static async Task<T> GetLocationBySerialNumberAsync<T>(this HttpClient client, string serialNumber) where T : class
        {
            var requestUri = $"/api/{typeof(T).Name}/getlocation/{serialNumber}";

            T data = null;
            var response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<T>(responseString);
            }
            return data;
        }
    }
}
