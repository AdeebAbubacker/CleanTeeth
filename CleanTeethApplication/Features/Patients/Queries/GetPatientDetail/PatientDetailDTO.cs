using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Queries.GetPatientDetail
{
    public class PatientDetailDTO
    {
        public required Guid Id { get; set; }
        public required String Name { get; set; }  

        public required String Email { get; set; }
    }
}
