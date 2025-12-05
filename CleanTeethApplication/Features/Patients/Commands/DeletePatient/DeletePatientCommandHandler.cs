using CleanTeeth.Application.Contracts.Repositories;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Commands.DeletePatient
{
    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand>
    {
        private readonly IPatientRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public DeletePatientCommandHandler(IPatientRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(DeletePatientCommand request)
        {
            var patient = await repository.GetById(request.Id);
            if (patient == null)
            {
                throw new NotFoundException();
            }
            try
            {
                await repository.Delete(patient);
                await unitOfWork.Commit();

            }
            catch (Exception)
            {
                await unitOfWork.Rollback();
                throw;

            }
        }
    }
}
