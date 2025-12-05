using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeList
{
    public class GetDentalOfficeListQueryHandler : IRequestHandler<GetDentalOfficeListQuery, List<GetDentalOfficeListDTO>>
    {
        private readonly IDentalOfficeRepository repository;

        public GetDentalOfficeListQueryHandler(IDentalOfficeRepository repository)
        {
            this.repository = repository;
        }
       
        public async Task<List<GetDentalOfficeListDTO>> Handle(GetDentalOfficeListQuery request)
        {
            var dentalOffices = await repository.GetAll();
            var dentalOfficeDTO = dentalOffices.Select(dentalOffice => dentalOffice.ToDTO()).ToList();
            return dentalOfficeDTO;
        }
    }
}
