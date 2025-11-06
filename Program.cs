using System;
using System.Net.Http;
using System.Threading.Tasks;

using BreviumScheduler.Models;
using BreviumScheduler.Services;

namespace BreviumScheduler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var coordinator = new SchedulingCoordinator();
            await coordinator.ScheduleAppointmentsAsync(args);
        }
    }
}