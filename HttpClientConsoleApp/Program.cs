using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;

namespace HttpClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //HttpClient h = new HttpClient();
            //var s = h.GetAsync("http://www.google.com");
            //foreach (var header in s.Result.Headers)
            //{
            //    Console.WriteLine(header.Key);
            //    foreach (var val in header.Value)
            //        Console.WriteLine(" " + val);
            //}

            //Task s= WeatherForecast("").ContinueWith((x) => { GetLocalAPI(); }).ContinueWith((x) => { PostCallLocalAPISimpleString(); });
            Task s = PostImagesLocalAPI();
                s.Wait();
                 Console.ReadLine();
        }

        /// <summary>
        /// TPM style coding
        /// </summary>
        /// <param name="searchString"></param>
        static async Task WeatherForecast(string searchString)
        {
            HttpClient h = new HttpClient();
            //Get Current Weather
            var s = h.GetAsync("http://api.openweathermap.org/data/2.5/weather?id=2172797&APPID=06f9b0a623dd6813e5353913c7b89874").ContinueWith((x) =>
                { x.Result.Content.ReadAsStringAsync().ContinueWith((json) => { Console.WriteLine(json.Result); }); }
                );

           


            HttpRequestMessage Requestmessage = new HttpRequestMessage();
            Requestmessage.Method = HttpMethod.Get;
            Requestmessage.RequestUri = new Uri("http://api.openweathermap.org/data/2.5/weather?id=2172797&APPID=06f9b0a623dd6813e5353913c7b89874");
            await h.SendAsync(Requestmessage).ContinueWith((x) =>
            { x.Result.Content.ReadAsStringAsync().ContinueWith((json) => { Console.WriteLine(json.Result); }); }
                );

            Console.WriteLine("--------------------------------------------------------------------------------\n");

        }

        static async void GetLocalAPI()
        {
            Console.WriteLine("--------------------------GET CALL LOcAL API------------------------------------------------------\n");
            HttpClient h = new HttpClient();          
            
            HttpRequestMessage Requestmessage = new HttpRequestMessage();
            Requestmessage.Method = HttpMethod.Get;
            Requestmessage.RequestUri = new Uri("http://localhost:1284/api/values");
            await h.SendAsync(Requestmessage).ContinueWith((x) =>
            { x.Result.Content.ReadAsStringAsync().ContinueWith((json) => { Console.WriteLine(json.Result); }); }
                );

            Console.WriteLine("--------------------------------------------------------------------------------\n");

        }
        static async void PostCallLocalAPISimpleString()
        {
            Console.WriteLine("----------------------------POST Call Local API----------------------------------------------------\n");
            HttpClient h = new HttpClient();
            //h.DefaultRequestHeaders.Accept.Add(
            //     new MediaTypeWithQualityHeaderValue("application/json"));

           await h.PostAsync("http://localhost:1284/api/values", new StringContent("\" Kishore \"", Encoding.UTF8, "application/json")).ContinueWith((x) =>
           { x.Result.Content.ReadAsStringAsync().ContinueWith((json) => { Console.WriteLine(json.Result); }); }
                ); 

           



            Console.WriteLine("--------------------------------------------------------------------------------\n");
        }
        static async Task PostCallLocalAPIModel()
        {
            Console.WriteLine("----------------------------POST Call Local API----------------------------------------------------\n");
            HttpClient h = new HttpClient();
            MyModel m = new MyModel() { ID = 1, Data = "Chulubhar Pani mein Dub Mar" };

            string jsonobject = JsonConvert.SerializeObject(m);

            await h.PostAsync("http://localhost:1284/api/values", new StringContent(jsonobject, Encoding.UTF8, "application/json")).ContinueWith((x) =>
            { x.Result.Content.ReadAsStringAsync().ContinueWith((json) => { Console.WriteLine(json.Result); }); }
                 );





            Console.WriteLine("--------------------------------------------------------------------------------\n");
        }

        static async Task PostImagesLocalAPI()
        {
            using (var stream = File.OpenRead(@"D:\PD\WebAPI_NET\WebAPIs_Lab\HttpClientConsoleApp\Images\Untitled.png"))
            {
                var client = new HttpClient();
             
                var response = await client.PostAsync("http://localhost:26140/api/images", new StreamContent(stream));
                response.EnsureSuccessStatusCode();
            }
        }

    }

    public class MyModel
    {
        public int ID { get; set; }

        public string Data { get; set; }
    }
}
