using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidaator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidaator() {
            RuleFor(x => x.StartDate).GreaterThan(x=>x.EndDate).WithMessage("Start date must be before end date");
        }
    }
}
