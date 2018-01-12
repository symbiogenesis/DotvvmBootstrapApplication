using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RingDownConsole.Interfaces;

namespace RingDownConsole.Utils.Extensions
{

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
