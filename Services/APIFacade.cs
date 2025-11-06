using System.Net;
using System.Text;
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
            try
            {
                var res = await _http.PostAsync($"api/Scheduling/Start?token={_apiKey}", null);
                res.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to start schedule: {ex.Message}", ex);
            }
        }

        public async Task<Schedule> StopAsync()
        {
            try
            {
                var res = await _http.PostAsync($"api/Scheduling/Stop?token={_apiKey}", null);
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                var appointments = JsonSerializer.Deserialize<List<AppointmentInfo>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (appointments == null || appointments.Count == 0)
                {
                    throw new Exception("No appointments found in the schedule response.");
                }

                return new Schedule { Appointments = appointments };
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve schedule from Stop API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to parse stop API schedule response: {ex.Message}", ex);
            }
        }

        public async Task<Schedule> GetScheduleAsync()
        {
            try
            {
                var res = await _http.GetAsync($"api/Scheduling/Schedule?token={_apiKey}");
                res.EnsureSuccessStatusCode();

                
                var json = await res.Content.ReadAsStringAsync();

                var appointments = JsonSerializer.Deserialize<List<AppointmentInfo>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (appointments == null || appointments.Count == 0)
                {
                    throw new Exception("No appointments found in the schedule response.");
                }

                return new Schedule { Appointments = appointments };
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

        public async Task<AppointmentRequest?> GetNextRequestAsync()
        {
            try
            {
                var res = await _http.GetAsync($"api/Scheduling/AppointmentRequest?token={_apiKey}");

                if (res.StatusCode == HttpStatusCode.NoContent)
                {
                    Console.WriteLine("No more appointment requests in queue.");
                    return null;
                }

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                var appointment = JsonSerializer.Deserialize<AppointmentRequest>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return appointment;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve appointment from API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to parse appointment response: {ex.Message}", ex);
            }
        }

        public async Task PostAppointmentAsync(AppointmentInfoRequest appointment)
        {
            try
            {
                // Serialize the appointment to JSON
                var json = JsonSerializer.Serialize(appointment);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request
                var res = await _http.PostAsync($"api/Scheduling/Schedule?token={_apiKey}", content);

                // Throw if status is not 2xx
                res.EnsureSuccessStatusCode();

                Console.WriteLine($"Appointment scheduled: RequestId {appointment.RequestId}, Doctor {appointment.DoctorId}, Person {appointment.PersonId} at {appointment.AppointmentTime:yyyy-MM-dd HH:mm}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to post appointment: {ex.Message}", ex);
            }
        }
    }
}