using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace XebecPortal.UI.Utils.Handlers
{
       
        public static class CustomHandler
        {
            public static async Task<string> GetJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization) // This is also for a GET request, however the return type is only for a string
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = authorization;

                var response = await httpClient.SendAsync(request);
                var responseBytes = await response.Content.ReadAsStringAsync();
                return responseBytes;
                //return JsonSerializer.Parse<T>(responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }

            public static async Task<T> GetListJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization) // Please note that this is used for GET requests. The variable receiving the return type needs to be a list of some sort.
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url); // Specifying the method type and the link API endpoint
                request.Headers.Authorization = authorization; // Adding the Authorization header, which is the JWT Token

                var response = await httpClient.SendAsync(request); // sending the request 
                var responseBytes = await response.Content.ReadAsByteArrayAsync();

                return JsonSerializer.Deserialize<T>(responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }


        // This is the how the original code for PostAsJsonAsync looks like
        //public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T value, CancellationToken cancellationToken)
        //{
        //    return client.PostAsync(requestUri, value, new JsonMediaTypeFormatter(), cancellationToken);
        //}

            public static async Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, string url, T value, AuthenticationHeaderValue authorization) // Note that this is for POSTING Lists, meaning that the receiving variable needs to be a list
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                httpClient.DefaultRequestHeaders.Authorization = authorization;  // test if this works
                //request.Headers.Authorization = authorization;                


                //var response = await httpClient.PostAsync(url, value, new JsonMediaTypeFormatter());
                //var contents = await response.Content.ReadAsHttpResponseMessageAsync();
                //var response = await httpClient.PostAsync(request); // sending the request
                return await httpClient.PostAsync(url, value, new JsonMediaTypeFormatter());
            }

            public static async Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, string url, T value, AuthenticationHeaderValue authorization) // Note that this is for POSTING Lists, meaning that the receiving variable needs to be a list
            {
                var request = new HttpRequestMessage(HttpMethod.Put, url);
                httpClient.DefaultRequestHeaders.Authorization = authorization;  // test if this works    
            //request.Headers.Authorization = authorization;

                //var response = await httpClient.SendAsync(request); // sending the request
                return await httpClient.PutAsync(url, value, new JsonMediaTypeFormatter());
            }

            public static Task<HttpResponseMessage> DeleteJsonAsync(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization) // Note that this is for POSTING Lists, meaning that the receiving variable needs to be a list
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                httpClient.DefaultRequestHeaders.Authorization = authorization;  // test if this works
            //request.Headers.Authorization = authorization;
                return httpClient.DeleteAsync(url);
            }
    }
        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQW5kcmV3IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9zZXJpYWxudW1iZXIiOiIxMDYiLCJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwiZW1haWwiOiJ0ZXN0QHRlc3QuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ2FuZGlkYXRlIiwianRpIjoiYzYwYjZhMDMtZDQ3Mi00N2EwLTlkMmUtYmUzZTk3YWI5YjIwIiwiZXhwIjoxNjQ5MzIxMzg5LCJpc3MiOiJkb21haW4uY29tIiwiYXVkIjoiZG9tYWluLmNvbSJ9.tVl0km7IvvvvsRoRgmZsz6HyUYSuw9dYNw3uw56JKmQ"; //_localStorage.GetItemAsync<string>("jwt_token");
        //    Console.WriteLine("Token " + token);
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    return base.SendAsync(request, cancellationToken);
        //}

        //public ILocalStorageService _localStorage { get; }
        //public HttpClient _httpClient { get; }

        //private PersonalInformation personalInformation = new();

        //public CustomHandler(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}
        //public async Task<string> newwwwtestJTW(string apiEndpoint,string token)
        //{
        //    // Can get token here to prevent devs from getting the token on their page
        //    using (var request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint))
        //    {
        //       // var newtoken = await _localStorage.GetItemAsync<string>("jwt_token");
        //       // Console.WriteLine("Token from a different page" + newtoken);
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

        //public async Task<List<PersonalInformation>> personalInfo(string apiEndpoint, string token)
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

        //        return null;//await response.Content.ReadAsStringAsync();
        //    }
        //}

    }

