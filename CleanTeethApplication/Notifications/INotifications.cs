using CleanTeeth.Application.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Notifications
{
    public interface INotifications
    {
        Task SendAppointmentConfimration(AppointmentConfirmationDTO appointmentConfirmationDTO);
    }
}
