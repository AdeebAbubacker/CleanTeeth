using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Queries.GetPatientsList
{
    public class PatientFilterDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;

        public string? Name { get; set; }

        public string? Email { get; set; }

    }
}
