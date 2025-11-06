using System;
using System.Net.Http;
using System.Threading.Tasks;
using BreviumScheduler.Models;

namespace BreviumScheduler.Services
{
    public class SchedulingCoordinator
    {
        private readonly ApiFacade _apiFacade;
        private readonly SchedulingRules _schedulingRules = new SchedulingRules();

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

            AppointmentRequest? request;

            while ((request = await _apiFacade.GetNextRequestAsync()) != null)
            {
                    var validAppointment = _schedulingRules.NextValidAppointment(request);
                    if (validAppointment == null)
                    {
                        Console.WriteLine($"Could not find valid slot for RequestId {request.RequestId}");
                        continue;
                    }

                await _apiFacade.PostAppointmentAsync(validAppointment);
                Console.WriteLine($"Submitted appointment for PersonId: {validAppointment.PersonId}");
            }

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