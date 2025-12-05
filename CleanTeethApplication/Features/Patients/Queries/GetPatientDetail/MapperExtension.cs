using CleanTeeth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Queries.GetPatientDetail
{
    internal static class MapperExtension
    {
        internal static PatientDetailDTO ToDTO(this Patient patient)
        {
            return new PatientDetailDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                Email = patient.Email.Value,
            };
        }
    }
}
