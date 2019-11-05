using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExceptionTest
{
    public static class AsyncClient
    {
        private static HttpClient HttpClient = new HttpClient();
        private static int count = 0;


      public  static async Task<string> GetResponseAsync()
        {
            int sequenceNumber = ++count;
            Debug.WriteLine($"Request #{sequenceNumber} started");
            string path = @"https://reqres.in/api/users?page=2";
            // HttpResponseMessage response = await HttpClient.GetAsync(path);
             var response =await HttpClient.GetAsync(path);
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                await Task.Delay(200);
            }

            Debug.WriteLine($"Request #{sequenceNumber} completed");
            return result;
        }
    

    public  static async void  GetResponseVoid()
        {
            int sequenceNumber = ++count;
            Debug.WriteLine($"Void Request #{sequenceNumber} started");
            string path = @"https://reqres.in/api/users?page=2";
            HttpResponseMessage response = await HttpClient.GetAsync(path);
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                 Debug.WriteLine($"Void Request waiting #{sequenceNumber} started");
                await Task.Delay(500);
            }

            Debug.WriteLine($"Void Request #{sequenceNumber} completed");
            
        }

        public  static async Task  GetResponseTask()
        {
            int sequenceNumber = ++count;
            Debug.WriteLine($"Task Request #{sequenceNumber} started");
            string path = @"https://reqres.in/api/users?page=2";
            HttpResponseMessage response = await HttpClient.GetAsync(path);
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                await Task.Delay(1000);
            }

            Debug.WriteLine($"Void Request #{sequenceNumber} completed");
            return;
            
        }

        
      public  static string GetResponse2()
        {
            int sequenceNumber = ++count;
            Debug.WriteLine($"Request #{sequenceNumber} started");
            string path = @"https://reqres.in/api/users?page=2";
            // HttpResponseMessage response = await HttpClient.GetAsync(path);
             var response = HttpClient.GetAsync(path).Result;
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
               // await Task.Delay(1000);
            }

            Debug.WriteLine($"Request #{sequenceNumber} completed");
            return result;
        }
    
    }
}
