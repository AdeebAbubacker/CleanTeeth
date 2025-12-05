using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.DentalOffices.Commands.UpdateDentalOffice
{
    public class UpdateDentalOfficeCommandHandler : IRequestHandler<UpdateDentalOfficeCommand>
    {
        private readonly IDentalOfficeRepository dentalOfficeRepository;
        private readonly IUnitOfWork unitofWork;

        public UpdateDentalOfficeCommandHandler(IDentalOfficeRepository dentalOfficeRepository,IUnitOfWork unitofWork)
        {
            this.dentalOfficeRepository = dentalOfficeRepository;
            this.unitofWork = unitofWork;
        }
        public async Task Handle(UpdateDentalOfficeCommand request)
        {
            var dentalOffice = await dentalOfficeRepository.GetById(request.Id);
            if (dentalOffice == null) {
            throw new NotFoundException();
            }
            dentalOffice.UpdateName(request.Name);
            try
            {
                await dentalOfficeRepository.Update(dentalOffice);
                await unitofWork.Commit();
            }
            catch (Exception) {
            await unitofWork.Rollback();
            throw;
            }
        }
    }
}
