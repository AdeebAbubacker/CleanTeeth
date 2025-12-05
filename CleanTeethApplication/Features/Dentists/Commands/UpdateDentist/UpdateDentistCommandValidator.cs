using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Dentists.Commands.UpdateDentist
{
    public class UpdateDentistCommandValidator : AbstractValidator<UpdateDentistCommand>
    {
        public UpdateDentistCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("The field {propertyName} is required");

            RuleFor(p => p.email)
     .EmailAddress()
     .WithMessage("Invalid email format");

        }
    }
}

