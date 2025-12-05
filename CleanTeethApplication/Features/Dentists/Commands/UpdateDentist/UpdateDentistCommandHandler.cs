using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities.ValueObjects;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Features.Patients.Commands.UpdatePatient;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Dentists.Commands.UpdateDentist
{
    public class UpdateDentistCommandHandler : IRequestHandler<UpdateDentistCommand>
    {
        private readonly IDentistRepositories patientRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateDentistCommandHandler(IDentistRepositories patientRepository, IUnitOfWork unitOfWork)
        {
            this.patientRepository = patientRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateDentistCommand request)
        {
            var patient = await patientRepository.GetById(request.Id);
            if (patient == null)
            {
                throw new NotFoundException();
            }
            patient.UpdateName(request.Name);
            var email = new Email(request.email);

            patient.UpdateEmail(email);
            try
            {
                await patientRepository.Update(patient);
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

//namespace CleanTeethApplication.Features.Patients.Commands.UpdatePatient
//{
//    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
//    {
//        private readonly IPatientRepository patientRepository;
//        private readonly IUnitOfWork unitOfWork;

//        public UpdatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
//        {
//            this.patientRepository = patientRepository;
//            this.unitOfWork = unitOfWork;
//        }
//        public async Task Handle(UpdatePatientCommand request)
//        {
//            var patient = await patientRepository.GetById(request.Id);
//            if (patient == null)
//            {
//                throw new NotFoundException();
//            }
//            patient.UpdateName(request.Name);
//            var email = new Email(request.Email);
//            patient.UpdateEmail(email);
//            try
//            {
//                await patientRepository.Update(patient);
//                await unitOfWork.Commit();
//            }
//            catch (Exception)
//            {
//                await unitOfWork.Rollback();
//                throw;
//            }
//        }
//    }
//}
