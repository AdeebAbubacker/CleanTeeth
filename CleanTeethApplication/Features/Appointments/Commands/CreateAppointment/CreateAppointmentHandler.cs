using CleanTeeth.Application.Features.Appointments.Commands.CreateAppointment;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Entities.ValueObjects;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Notifications;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Appointments.Commands.CreateAppointment
{

    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IAppointmentRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotifications notifications;

        public CreateAppointmentCommandHandler(IAppointmentRepository repository,
            IUnitOfWork unitOfWork,INotifications notifications)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.notifications = notifications;
        }

        public async Task<Guid> Handle(CreateAppointmentCommand request)
        {
            var existsOverlap = await repository.OverlapExists(request.DentistId, request.StartDate,
                request.EndDate);

            if (existsOverlap)
            {
                throw new CustomValidationException("The dentist have an appointment that overlaps");
            }

            var timeInterval = new TimeInterval(request.StartDate, request.EndDate);
            var appointment = new Appointment(request.patientId, request.DentistId, request.DentalOfficeId,
                timeInterval);

            Guid? id = null;

            try
            {
                var result = await repository.Add(appointment);
                await unitOfWork.Commit();
                id = result.Id;

            }
            catch (Exception)
            {
                await unitOfWork.Rollback();
                throw;
            }
            var appointmentDb = await repository.GetById(id.Value);
            var notificationDTO = appointmentDb.ToDto();
            await notifications.SendAppointmentConfimration(notificationDTO);
            return id.Value;
        }
    }

}
