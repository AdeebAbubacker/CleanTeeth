
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Entities.ValueObjects;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
    {
        private readonly IPatientRepository patientRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreatePatientCommandHandler(IPatientRepository patientRepository,IUnitOfWork unitOfWork)
        {
            this.patientRepository = patientRepository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<Guid> Handle(CreatePatientCommand request)
        {
            var email = new Email(request.Email);
            var patient = new Patient(request.Name, email);
            try
            {
            var result = await patientRepository.Add(patient);
            await unitOfWork.Commit();
            return result.Id;
            }
            catch (Exception) { 
            await unitOfWork.Rollback();
            throw;
            }
        }



    }
   
}
