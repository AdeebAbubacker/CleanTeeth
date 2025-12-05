using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.DentalOffices.Commands.DeleteDentalOffice
{
    public class DeleteDentalOfficeCommand : IRequest
    {
        public required Guid Id { get; set; }
    }
}
