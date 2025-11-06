using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BreviumScheduler
{
    public class Program
    {
        private static readonly HttpClient _http = new();
        public static async Task Main(string[] args)
        {
            // Load API key
            var apiKey = Environment.GetEnvironmentVariable("API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("Missing API_KEY.");
                return;
            }

            // Configure HTTP client
            _http.BaseAddress = new Uri("https://scheduling.interviews.brevium.com/");

            try
            {
                var startResponse = await _http.PostAsync($"api/Scheduling/Start?token={apiKey}", null);
                Console.WriteLine($"Start status: {(int)startResponse.StatusCode} {startResponse.ReasonPhrase}");

                var scheduleResponse = await _http.GetAsync($"api/Scheduling/Schedule?token={apiKey}");
                Console.WriteLine($"Schedule status: {(int)scheduleResponse.StatusCode} {scheduleResponse.ReasonPhrase}");

                var body = await scheduleResponse.Content.ReadAsStringAsync();
                Console.WriteLine("\n--- Response Body (first 500 chars) ---");
                Console.WriteLine(body[..Math.Min(500, body.Length)]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Request failed: {ex.Message}");
            }
        }
    }
}