using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Notifications
{
    public class NotificationConfirmationDTO
    {
        public required Guid Id { get; set; }

        public required String Patient {  get; set; }
        public required String Patient_Email { get; set; }
        public required String Dentist { get; set; }
        public required String DentalOffice { get; set; }
        public required DateTime Date { get; set; }
    }
}
