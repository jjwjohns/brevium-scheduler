using System;
using System.Threading.Tasks;
using BreviumScheduler.Models;

namespace BreviumScheduler.Services
{
    public class SchedulingCoordinator
    {
        private readonly ISchedulingApi _apiFacade;
        private Schedule _schedule = new Schedule();
        private readonly ISchedulingRules _schedulingRules;

        public SchedulingCoordinator(ISchedulingApi apiFacade, ISchedulingRules schedulingRules)
        {
            _apiFacade = apiFacade ?? throw new ArgumentNullException(nameof(apiFacade));
            _schedulingRules = schedulingRules ?? throw new ArgumentNullException(nameof(schedulingRules));
        }
        public async Task ScheduleAppointmentsAsync()
        {
        try
        {
            await _apiFacade.StartAsync();
            var schedule = await _apiFacade.GetScheduleAsync();
                Console.WriteLine($"Got {schedule.Appointments.Count} appointments");
            
            _schedule = schedule;
            AppointmentRequest? request;

            while ((request = await _apiFacade.GetNextRequestAsync()) != null)
            {
                var validAppointment = _schedulingRules.NextValidAppointment(request, _schedule);
                if (validAppointment == null)
                {
                    Console.WriteLine($"Could not find valid slot for RequestId {request.RequestId}");
                    continue;
                }

                await _apiFacade.PostAppointmentAsync(validAppointment);
                Console.WriteLine($"Submitted appointment for PersonId: {validAppointment.PersonId}");
                
                _schedule.Appointments.Add(
                    new AppointmentInfo
                    {
                        DoctorId = validAppointment.DoctorId,
                        PersonId = validAppointment.PersonId,
                        AppointmentTime = validAppointment.AppointmentTime,
                        IsNewPatientAppointment = validAppointment.IsNewPatientAppointment
                    }
                );
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