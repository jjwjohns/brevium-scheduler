using System.Threading.Tasks;
using BreviumScheduler.Models;

namespace BreviumScheduler.Services;

public interface ISchedulingApi
{
    Task StartAsync();
    Task StopAsync();
    Task<Schedule> GetScheduleAsync();
    Task<string?> GetNextRequestAsync();
    Task PostAppointmentAsync(string appointmentJson);
}