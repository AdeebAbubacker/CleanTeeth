using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Dentists.Commands.DeleteDentist
{
    public class DeleteDentistCommand : IRequest
    {
        public required Guid Id { get; set; }
    }
}

