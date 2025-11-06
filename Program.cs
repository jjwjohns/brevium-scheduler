using System;
using System.Net.Http;
using System.Threading.Tasks;

using BreviumScheduler.Models;
using BreviumScheduler.Services;

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

            var apiFacade = new ApiFacade(_http, apiKey);

        try
        {
            await apiFacade.StartAsync();
            var schedule = await apiFacade.GetScheduleAsync();
            Console.WriteLine($"Got {schedule.Appointments.Count} appointments");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        }
    }
}