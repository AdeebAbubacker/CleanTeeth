using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.DentalOffices.Commands.CreateDentalOffice
{
    public class CreateDentalOfficeCommandValidator : AbstractValidator<CreateDentalOfficeCommand>
    {
        public CreateDentalOfficeCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("The field {ProertyName} is required");
        }
    }
}
