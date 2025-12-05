using CleanTeeth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Dentists.Queries.GetDentistList
{
    internal static class mapperextensions
    {
        public static GetDentistListDTO ToDTO(this Dentist dentalOffice)
        {
            var dto = new GetDentistListDTO
            {
                Id = dentalOffice.Id,
                Name = dentalOffice.Name,
                Email = dentalOffice.Email.Value
            };
            return dto;
        }
    }
}

