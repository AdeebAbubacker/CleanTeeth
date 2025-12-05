using CleanTeeth.Domain.Entities.ValueObjects;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Dentists.Commands.UpdateDentist
{
    public class UpdateDentistCommand : IRequest
    {
        public required Guid Id { get; set; }
        public required String Name { get; set; }

        public  String email { get; set; }
    }
}


