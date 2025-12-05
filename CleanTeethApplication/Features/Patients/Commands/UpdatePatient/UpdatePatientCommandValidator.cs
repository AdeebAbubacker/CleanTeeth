using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Commands.UpdatePatient
{
    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator() {
        RuleFor(p=>p.Name).NotEmpty().WithMessage("The field {propertyName} is required");

            RuleFor(p => p.Email).NotEmpty().WithMessage("The field {propertyName} is required").EmailAddress().WithMessage("Invalid email format");
        
        }
    }
}
