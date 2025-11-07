using BreviumScheduler.Models;

namespace BreviumScheduler.Services;

public interface ISchedulingRules
{
    AppointmentInfoRequest? NextValidAppointment(AppointmentRequest request, Schedule schedule);
}