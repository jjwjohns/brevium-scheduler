using System.Threading.Tasks;
using BreviumScheduler.Models;

namespace BreviumScheduler.Services;

public interface ISchedulingApi
{
    Task StartAsync();
    Task<Schedule> StopAsync();
    Task<Schedule> GetScheduleAsync();
    Task<AppointmentInfoRequest?> GetNextRequestAsync();
    Task PostAppointmentAsync(AppointmentInfoRequest appointment);
}