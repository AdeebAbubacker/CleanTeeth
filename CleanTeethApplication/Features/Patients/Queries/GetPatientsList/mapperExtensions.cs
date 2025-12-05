using CleanTeeth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Queries.GetPatientsList
{
    internal static class mapperExtensions
    {
        internal static PatientListDTO ToDto(this Patient patient) {
            return new PatientListDTO
            {
                Id = patient.Id,
                Email = patient.Email.Value,
                Name = patient.Name,
            };
        }
    }
}
