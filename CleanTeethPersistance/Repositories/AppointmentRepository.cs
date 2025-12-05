using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Entities.Enums;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Features.Appointments.Queries.GetAppointmentList;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethPersistance.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private readonly CleanTeethDbContext context;

        public AppointmentRepository(CleanTeethDbContext context) : base(context) 
        {
            this.context = context;
        }

        public  async Task<bool> OverlapExists(Guid dentistId, DateTime start, DateTime end)
        {
           return await context.Appoinments.Where(x=>x.DentistId == dentistId &&x.Status ==
           AppointmentStatus.Scheduled  && start < x.TimeInterval.End && end > x.TimeInterval.Start).AnyAsync();
        }

        //----------we will call this get by id to query from multiple tables
        new public async Task<Appointment?> GetById(Guid id)
        {
            return await context.Appoinments
                .Include(x=>x.Patient)
                .Include(x=>x.Dentist)
                .Include(x=>x.DentalOffice)
                .FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetFiltered(AppointmentFilterDTO appointmentFilterDTO)
        {
            var queryable = context.Appoinments
                .Include(x => x.DentalOffice)
                .Include(x => x.Dentist)
                .Include(x => x.Patient)
                .AsQueryable();
            if(appointmentFilterDTO.DentalOfficeId is not null)
            {
                queryable = queryable.Where(x=>x.DentalOfficeId == appointmentFilterDTO.DentalOfficeId);
            }
            if (appointmentFilterDTO.PatientId is not null)
            {
                queryable = queryable.Where(x => x.PatientId == appointmentFilterDTO.PatientId);
            }
            if (appointmentFilterDTO.DentistId is not null)
            {
                queryable = queryable.Where(x => x.DentistId == appointmentFilterDTO.DentistId);
            }
            return await queryable.Where(x => x.TimeInterval.Start >= appointmentFilterDTO.StartDate
            && x.TimeInterval.End <= appointmentFilterDTO.EndDate)
                .OrderBy(x => x.TimeInterval.Start).ToListAsync();
   
        }
    }
}
