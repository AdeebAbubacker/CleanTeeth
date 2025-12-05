using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.DentalOffices.Commands.UpdateDentalOffice
{
    public class UpdateDentalOfficeCommand : IRequest
    {
        public required Guid Id { get; set; }
        public string Name { get; set; }
    }
}
