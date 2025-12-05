using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Features.Patients.Queries.GetPatientsList;
using CleanTeethPersistance.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethPersistance.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository    
    {
        private readonly CleanTeethDbContext context;

        public PatientRepository(CleanTeethDbContext context) : base(context){
            this.context = context;
        }

        public  async Task<IEnumerable<Patient>> GetFiltered(PatientFilterDTO filterDTO)
        {
            var queryable = context.Patients.AsQueryable();
            if(!string.IsNullOrWhiteSpace(filterDTO.Name))
                {
                queryable = queryable.Where(x => x.Name.Contains(filterDTO.Name));
            }
            if (!string.IsNullOrWhiteSpace(filterDTO.Email))
            {
                queryable = queryable.Where(x => x.Email.Value.Contains(filterDTO.Email));
            }
            return await queryable.OrderBy(x => x.Name).Paginate(filterDTO.Page,filterDTO.RecordsPerPage).ToListAsync();
        }
    }
}
