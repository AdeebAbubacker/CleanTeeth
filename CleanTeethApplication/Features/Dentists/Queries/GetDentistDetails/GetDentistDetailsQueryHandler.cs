
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Features.Dentists.Queries.GetDentistList;
using CleanTeethApplication.Features.Patients.Queries.GetPatientDetail;
using CleanTeethApplication.Utilities;



namespace CleanTeethApplication.Features.Dentists.Queries.GetDentistDetails
{
    public class GetDentistDetailQueryHandler : IRequestHandler<GetDentistDetailQuery, GetDentistDetailDTO>
    {
        private readonly IDentistRepositories repository;

        public GetDentistDetailQueryHandler(IDentistRepositories repository)
        {
            this.repository = repository;
        }

        public async Task<GetDentistDetailDTO> Handle(GetDentistDetailQuery request)
        {
            var dentist = await repository.GetById(request.Id);
            if (dentist == null)
            {
                throw new DirectoryNotFoundException();
            }
            return dentist.ToDTO();
        }

    }
}


