using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Appointments.Queries.GetAppointmentList
{
    public class GetAppointmentsListQuery: AppointmentFilterDTO, IRequest<List<AppointmentListDTO>>
    {
    }
}
