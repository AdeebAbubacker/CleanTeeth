using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Features.Dentists.Queries.GetDentistList;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CleanTeethApplication.Features.DentalOffices.Queries.GetDentistList
{
    public class GetDentistListQueryHandler : IRequestHandler<GetDentistListQuery, List<GetDentistListDTO>>
    {
        private readonly IDentistRepositories repository;

        public GetDentistListQueryHandler(IDentistRepositories repository)
        {
            this.repository = repository;
        }

        public async Task<List<GetDentistListDTO>> Handle(GetDentistListQuery request)
        {
            var dentalOffices = await repository.GetAll();
            var dentalOfficeDTO = dentalOffices.Select(dentalOffice => dentalOffice.ToDTO()).ToList();
            return dentalOfficeDTO;
        }
    }
}
