using CleanTeeth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CleanTeethApplication.Features.Dentists.Queries.GetDentistDetails
{
    internal static class mapperextensions
    {
        public static GetDentistDetailDTO ToDTO(this Dentist dentalOffice)
        {
            var dto = new GetDentistDetailDTO
            {
                Id = dentalOffice.Id,
                Name = dentalOffice.Name,
                Email = dentalOffice.Email.Value
            };
            return dto;
        }
    }
}

