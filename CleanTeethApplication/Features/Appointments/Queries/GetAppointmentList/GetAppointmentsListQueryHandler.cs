using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Utilities;
//using Microsoft.Analytics.Interfaces;
//using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CleanTeethApplication.Features.Appointments.Queries.GetAppointmentList
{
    public class GetAppointmentsListQueryHandler : IRequestHandler<GetAppointmentsListQuery, List<AppointmentListDTO>>
    {
        private readonly IAppointmentRepository repository;

        public GetAppointmentsListQueryHandler(IAppointmentRepository repository)
        {
            this.repository = repository;
        }
        public async Task<List<AppointmentListDTO>> Handle(GetAppointmentsListQuery request)
        {
            var appointments = await repository.GetFiltered(request);
            var appointmentsDTO = appointments.Select(appointment => appointment.ToDto()).ToList();
            return appointmentsDTO;
        }
    }
}