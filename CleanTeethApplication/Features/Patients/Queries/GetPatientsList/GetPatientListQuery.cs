using CleanTeethApplication.Utilities;
using CleanTeethApplication.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Queries.GetPatientsList
{
    public class GetPatientListQuery : PatientFilterDTO, IRequest<PaginatedDTO<PatientListDTO>>
    {
        
    }
}
