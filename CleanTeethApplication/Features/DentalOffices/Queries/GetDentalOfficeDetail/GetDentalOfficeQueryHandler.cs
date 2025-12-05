using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeDetail
{
    public class GetDentalOfficeQueryHandler : IRequestHandler<GetDentalOfficeDetailQuery, DentalOfficeDetailDTO>
    {
        private readonly IDentalOfficeRepository dentalOfficeRepository;

        public GetDentalOfficeQueryHandler(IDentalOfficeRepository dentalOfficeRepository)
        {
            this.dentalOfficeRepository = dentalOfficeRepository;
        }

        public async Task<DentalOfficeDetailDTO> Handle(GetDentalOfficeDetailQuery request)
        {
           var dentalOffice = await dentalOfficeRepository.GetById(request.Id);
            if (dentalOffice == null) {
                throw new NotFoundException();
            } 
            return dentalOffice.ToDto();
        }
    }
}
