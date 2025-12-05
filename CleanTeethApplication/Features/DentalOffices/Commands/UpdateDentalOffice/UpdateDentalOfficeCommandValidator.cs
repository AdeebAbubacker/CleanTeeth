using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.DentalOffices.Commands.UpdateDentalOffice
{
    public class UpdateDentalOfficeCommandValidator : AbstractValidator<UpdateDentalOfficeCommand>
    {
        public UpdateDentalOfficeCommandValidator() {
            RuleFor(p => p.Name).NotEmpty().WithMessage("The field {propertyName} is required");

        }
    }
}
