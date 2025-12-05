using CleanTeeth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeList
{
    internal static class MapperExtensions
    {
        public static GetDentalOfficeListDTO ToDTO(this DentalOffice dentalOffice)
        {
            var dto = new GetDentalOfficeListDTO
            {
                Id = dentalOffice.Id,
                Name = dentalOffice.Name
            };
            return dto;
        }
    }
}