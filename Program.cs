using BreviumScheduler.Services;

namespace BreviumScheduler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var apiKey = Environment.GetEnvironmentVariable("API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("Missing API_KEY.");
            }
            
            var _apiFacade = new ApiFacade(new HttpClient(), apiKey);
            var coordinator = new SchedulingCoordinator(_apiFacade);
            await coordinator.ScheduleAppointmentsAsync(args);
        }
    }
}