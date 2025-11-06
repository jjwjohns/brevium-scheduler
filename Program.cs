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

            var apiFacade = new ApiFacade(new HttpClient(), apiKey);
            Console.WriteLine("Starting Scheduling Coordinator...");
            var coordinator = new SchedulingCoordinator(apiFacade);
            await coordinator.ScheduleAppointmentsAsync(args);
        }
    }
}