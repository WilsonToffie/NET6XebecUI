using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace XebecPortal.UI.Utils.Handlers
{
    
    public class CustomHandler 
    {

        public ILocalStorageService _localStorage { get; }
        public HttpClient _httpClient { get; }

        public CustomHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> newwwwtestJTW(string apiEndpoint,string token)
        {
            // Can get token here to prevent devs from getting the token on their page
            Console.WriteLine("GEts in here");
            using (var request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint))
            {
                Console.WriteLine("GEts in here2");
                //var token = await _localStorage.GetItemAsync<string>("jwt_token");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.SendAsync(request);
                Console.WriteLine("GEts in here3");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("It was successfull");
                }
                // response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        //public async Task<List<>> personalInfo(string apiEndpoint, string token)
        //{
        //    // Can get token here to prevent devs from getting the token on their page
        //    Console.WriteLine("GEts in here");
        //    using (var request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint))
        //    {
        //        Console.WriteLine("GEts in here2");
        //        //var token = await _localStorage.GetItemAsync<string>("jwt_token");
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        var response = await _httpClient.SendAsync(request);
        //        Console.WriteLine("GEts in here3");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("It was successfull");
        //        }
        //        // response.EnsureSuccessStatusCode();

        //        return await response.Content.ReadAsStringAsync();
        //    }
        //}

    }
}
