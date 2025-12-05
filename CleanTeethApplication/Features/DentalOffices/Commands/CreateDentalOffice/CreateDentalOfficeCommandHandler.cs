using CleanTeeth.Domain.Entities;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Utilities;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.DentalOffices.Commands.CreateDentalOffice
{
    public class CreateDentalOfficeCommandHandler : IRequestHandler<CreateDentalOfficeCommand,Guid>
    {
        private readonly IDentalOfficeRepository repository;
        private readonly IUnitOfWork unitOfWork;


        public CreateDentalOfficeCommandHandler(IDentalOfficeRepository dentalrepository,IUnitOfWork unitOfWork)
        {
            this.repository = dentalrepository;
            this.unitOfWork = unitOfWork;
   
        }

        public async Task<Guid> Handle(CreateDentalOfficeCommand command)
        {
       
            var dentalOffice = new DentalOffice(command.Name);
            try
            {
                var result = await repository.Add(dentalOffice);
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

//Mediator was last video