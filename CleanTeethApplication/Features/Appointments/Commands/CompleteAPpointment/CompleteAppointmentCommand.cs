using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Appointments.Commands.CompleteAPpointment
{
    public class CompleteAppointmentCommand : IRequest
    {
        public required Guid Id { get; set; }

    }
}
