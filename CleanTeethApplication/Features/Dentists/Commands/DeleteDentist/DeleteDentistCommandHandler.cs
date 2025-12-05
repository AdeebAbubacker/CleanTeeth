using CleanTeeth.Application.Contracts.Repositories;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Dentists.Commands.DeleteDentist
{
    public class DeleteDentistCommandHandler : IRequestHandler<DeleteDentistCommand>
    {
        private readonly IDentistRepositories repository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteDentistCommandHandler(IDentistRepositories repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteDentistCommand request)
        {
            var dentist = await repository.GetById(request.Id);
            if (dentist == null)
            {
                throw new NotFoundException();
            }
            try
            {
                await repository.Delete(dentist);
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