using System;
using System.Net.Http;
using System.Threading.Tasks;
using BreviumScheduler.Models;

namespace BreviumScheduler.Services
{
    public class SchedulingCoordinator
    {
        private readonly ApiFacade _apiFacade;

        public SchedulingCoordinator(ApiFacade apiFacade)
        {
            _apiFacade = apiFacade;
        }
        public async Task ScheduleAppointmentsAsync(string[] args)
        {
        try
        {
            await _apiFacade.StartAsync();
            var schedule = await _apiFacade.GetScheduleAsync();
            Console.WriteLine($"Got {schedule.Appointments.Count} appointments");
            var finalSchedule = await _apiFacade.StopAsync();
            Console.WriteLine($"Final schedule has {finalSchedule.Appointments.Count} appointments");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        }
    }
}