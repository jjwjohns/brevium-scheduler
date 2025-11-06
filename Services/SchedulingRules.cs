using System;
using BreviumScheduler.Models;

namespace BreviumScheduler.Services
{
    public class SchedulingRules
    {
        public AppointmentInfoRequest NextValidAppointment(AppointmentRequest request)
        {
            var appointmentRequest = new AppointmentInfoRequest
            {
                DoctorId = Doctor.Doctor1,
                PersonId = 1,
                AppointmentTime = DateTime.Parse("2025-11-06T23:29:08.157Z").ToUniversalTime(),
                IsNewPatientAppointment = true,
                RequestId = request.RequestId
            };

            return appointmentRequest;
        }
    }
}