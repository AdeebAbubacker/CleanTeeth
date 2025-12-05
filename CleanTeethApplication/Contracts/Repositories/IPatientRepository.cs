using CleanTeeth.Domain.Entities;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Features.Patients.Queries.GetPatientsList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Application.Contracts.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<IEnumerable<Patient>> GetFiltered(PatientFilterDTO filter);
    }
}

