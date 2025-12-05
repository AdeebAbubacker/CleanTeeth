using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Commands.UpdatePatient
{
    public class UpdatePatientCommand : IRequest
    {
        public required Guid Id { get; set; }
        public required String Name { get; set; }

        public required String Email { get; set; }


    }
}
