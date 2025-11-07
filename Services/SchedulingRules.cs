using System;
using System.Linq;
using BreviumScheduler.Models;

namespace BreviumScheduler.Services
{
    public class SchedulingRules
    {
        private readonly DateTime start = new DateTime(2021, 11, 1, 8, 0, 0, DateTimeKind.Utc);
        private readonly DateTime end = new DateTime(2021, 12, 31, 16, 0, 0, DateTimeKind.Utc);

        public AppointmentInfoRequest? NextValidAppointment(AppointmentRequest request, Schedule calendar)
        {
            for (var time = start; time <= end; time = time.AddHours(1))
            {
                // ---- Validate core time window (hours & weekdays) ----
                if (time.Hour < 8 || time.Hour > 16) continue;
                if (time.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;

                // ---- Apply request-based constraints ----
                // New patients only at 3 PM or 4 PM
                if (request.IsNew && time.Hour < 15) continue;

                // Must match one of the preferred specific dates (if any provided)
                if (request.PreferredDays.Count > 0 &&
                    !request.PreferredDays.Any(d => d.ToUniversalTime().Date == time.Date))
                    continue;

                var doctorIds = request.PreferredDocs.Count > 0
                ? request.PreferredDocs
                : calendar.Appointments.Select(a => a.DoctorId).Distinct().ToList();

                // ---- Check each preferred doctor (or all doctors if no preference) ----
                foreach (var doc in doctorIds)
                {
                    // Skip if doctor already has an appointment at this time
                    var doctorBusy = calendar.Appointments.Exists(a =>
                        a.DoctorId == doc && a.AppointmentTime == time);
                    if (doctorBusy) continue;

                    // Skip if patient has another appointment within 7 days
                    var patientConflict = calendar.Appointments.Exists(a =>
                        a.PersonId == request.PersonId &&
                        Math.Abs((a.AppointmentTime - time).TotalDays) < 7);
                    if (patientConflict) continue;

                    // ---- If all checks pass, return next valid appointment ----
                    return new AppointmentInfoRequest
                    {
                        DoctorId = doc,
                        PersonId = request.PersonId,
                        AppointmentTime = time,
                        IsNewPatientAppointment = request.IsNew,
                        RequestId = request.RequestId
                    };
                }
            }
            // No valid appointment found
            return null;
        }
    }
}
