using CleanTeeth.Domain.Entities;
using CleanTeethApplication.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethPersistance.Repositories
{
    public class DentalOfficeRepository : Repository<DentalOffice>,IDentalOfficeRepository
    {
        public DentalOfficeRepository(CleanTeethDbContext context):base(context) { 
        
        
        }
    }
}
