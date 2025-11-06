using Microsoft.VisualBasic;
using System.Text.Json;
using BreviumScheduler.Models;

namespace BreviumScheduler.Services
{
    public class ApiFacade : ISchedulingApi
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public ApiFacade(HttpClient http, string apiKey)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://scheduling.interviews.brevium.com/");
            _apiKey = apiKey;
        }
        public async Task StartAsync()
        {
            // Implementation
            throw new NotImplementedException();
        }

        public async Task StopAsync()
        {
            // Implementation
            throw new NotImplementedException();
        }

        public async Task<Schedule> GetScheduleAsync()
        {
            try
            {
                var res = await _http.GetAsync($"api/Scheduling/Schedule?token={_apiKey}");
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Schedule>(json)!;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve schedule from API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to parse schedule response: {ex.Message}", ex);
            }
        }

        public async Task<string?> GetNextRequestAsync()
        {
            // Implementation
            throw new NotImplementedException();
        }

        public async Task PostAppointmentAsync(string appointmentJson)
        {
            // Implementation
            throw new NotImplementedException();
        }

    }
}