using System;
using BreviumScheduler.Models;


// Constraints:

// Appointments may only be scheduled on the hour. 
// Appointments can be scheduled as early as 8 am UTC and as late as 4 pm UTC. 
// Appointments may only be scheduled on weekdays during the months of November and December 2021. 
// Appointments can be scheduled on holidays. 

// For a given doctor, you may only have one appointment scheduled per hour (though different doctors may have appointments at the same time). 
// For a given patient, each appointment must be separated by at least one week. For example, if Bob Smith has an appointment on 11/17 you may schedule another appointment on or before 11/10 or on or after 11/24. 
// Appointments for new patients may only be scheduled for 3 pm and 4 pm. 

namespace BreviumScheduler.Services
{
    public class SchedulingRules
    {
        private DateTime start = new DateTime(2021, 11, 1, 8, 0, 0, DateTimeKind.Utc);
        private DateTime end = new DateTime(2021, 12, 31, 16, 0, 0, DateTimeKind.Utc);
        public AppointmentInfoRequest? NextValidAppointment(AppointmentRequest request, Schedule calendar)
        {
            for (var time = start; time <= end; time = time.AddHours(1))
            {
                // Only consider 8–16 and weekdays
                if (time.Hour < 8 || time.Hour > 16) continue;
                if (time.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;

                // Check each appointment constraint here
                if (request.IsNew && time.Hour < 15) continue;
                if (request.PreferredDays.Count > 0 &&
                    !request.PreferredDays.Any(d => d.Date == time.Date))
                    continue;
                    
                foreach (var doc in request.PreferredDocs)
                {
                    // Check if doctor is available at this time
                    bool doctorBusy = calendar.Appointments.Exists(a => a.DoctorId == doc && a.AppointmentTime == time);
                    if (doctorBusy) continue;

                    // Check if patient has another appointment within a week
                    bool patientConflict = calendar.Appointments.Exists(a =>
                        a.PersonId == request.PersonId &&
                        Math.Abs((a.AppointmentTime - time).TotalDays) < 7);
                    if (patientConflict) continue;

                    // If all checks passed, return this appointment
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
            return null;
        }

    }
}