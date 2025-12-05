using CleanTeeth.Domain.Entities;
using CleanTeethApplication.Features.Appointments.Queries.GetAppointmentList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Contracts.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<bool> OverlapExists(Guid dentistId,DateTime start,DateTime end);

        //This will override current get by id so  that we can query from differnt tables
        new Task<Appointment?> GetById(Guid id);
        Task<IEnumerable<Appointment>> GetFiltered(AppointmentFilterDTO appointmentFilterDTO);


    }
}
