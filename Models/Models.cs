using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BreviumScheduler.Models
{
    public enum Doctor { Doctor1 = 1, Doctor2 = 2, Doctor3 = 3 }

    public class AppointmentInfo
    {
        public Doctor DoctorId { get; set; }
        public int PersonId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public bool IsNewPatientAppointment { get; set; }
    }

    public class AppointmentInfoRequest : AppointmentInfo
    {
        public int RequestId { get; set; }
    }

    public class AppointmentRequest
    {
        public int RequestId { get; set; }
        public int PersonId { get; set; }
        public List<DayOfWeek> PreferredDays { get; set; } = new();
        public List<Doctor> PreferredDocs { get; set; } = new();
        public bool IsNew { get; set; }
    }

    public class Schedule
    {
        public List<AppointmentInfo> Appointments { get; set; } = new();
    }
}
