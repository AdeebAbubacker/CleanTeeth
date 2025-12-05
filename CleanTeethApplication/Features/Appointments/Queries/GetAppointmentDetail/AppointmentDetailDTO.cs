using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Appointments.Queries.GetAppointmentDetail
{
    public class AppointmentDetailDTO
    {
        public required Guid Id { get; set; }
        public required String Patient { get; set; }
        public required String Dentist { get; set; }
        public required String DentalOffice { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required String Status { get; set; }
    }
}
