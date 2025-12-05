using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Appointments.Commands.CompleteAPpointment
{
    public class CompleteAppointmnetCommandHandler : IRequestHandler<CompleteAppointmentCommand>
    {
        private readonly IAppointmentRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CompleteAppointmnetCommandHandler(IAppointmentRepository repository,IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(CompleteAppointmentCommand request)
        {
           var appointment = await repository.GetById(request.Id);
            if (appointment == null) {
                throw new NotFoundException();
            }
            appointment.Complete();
            try
            {
                await repository.Update(appointment);
                await unitOfWork.Commit();
            }
            catch (Exception) {
            await unitOfWork.Rollback();
                throw;
            }
        }
    }
}
