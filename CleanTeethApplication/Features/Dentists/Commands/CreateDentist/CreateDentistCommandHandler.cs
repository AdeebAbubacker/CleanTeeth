using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Entities.ValueObjects;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Features.Patients.Commands.CreatePatient;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Dentists.Commands.CreateDentist
{
    public class CreateDentistCommandHandler : IRequestHandler<CreateDentistCommand, Guid>
    {
        private readonly IDentistRepositories dentistRepositories;
        private readonly IUnitOfWork unitOfWork;

        public CreateDentistCommandHandler(IDentistRepositories dentistRepositories, IUnitOfWork unitOfWork)
        {
            this.dentistRepositories = dentistRepositories;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateDentistCommand request)
        {
            var email = new Email(request.Email);
            var patient = new Dentist(request.Name, email);
            try
            {
                var result = await dentistRepositories.Add(patient);
                await unitOfWork.Commit();
                return result.Id;
            }
            catch (Exception)
            {
                await unitOfWork.Rollback();
                throw;
            }
        }
    }
}

