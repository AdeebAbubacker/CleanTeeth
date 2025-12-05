

using CleanTeeth.Domain.Entities;
using CleanTeethApplication.Contracts.Repositories;

namespace CleanTeethPersistance.Repositories
{
    public class DentistRepository : Repository<Dentist>, IDentistRepositories
    {
        public DentistRepository(CleanTeethDbContext context) : base(context)
        {


        }
    }
}
