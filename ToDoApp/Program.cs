using System;
using Newtonsoft.Json;
using RestSharp;

namespace ToDoApp
{
    class Program
    {
        private static RestClient client = new RestClient("http://demo2.z-bit.ee/");
        static void Main(string[] args)
        {
            var request = new RestRequest("/users", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"username\": \"aleksander.altmae@tptlive.ee\",\r\n    \"firstname\": \"Aleksander\",\r\n    \"lastname\": \"Altmäe\",\r\n    \"newPassword\": \"aleksander\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            Login("aleksander.altmae@tptlive.ee", "aleksander");
        }

        public static void Login(string user, string pass)
        {
            var request = new RestRequest("/users/get-token", Method.POST);
            request.AddJsonBody(new { username = user, password = pass });
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            dynamic answer = JsonConvert.DeserializeObject(response.Content);
            string token = answer.access_token;
            CreateTask(token);
        }

        public static void CreateTask(string token)
        {
            var request = new RestRequest("/tasks", Method.POST);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddJsonBody(new { title = "Järgmine ülesanne" });
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
