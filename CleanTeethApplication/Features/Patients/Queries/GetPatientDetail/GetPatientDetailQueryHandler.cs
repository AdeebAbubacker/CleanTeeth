using CleanTeeth.Application.Contracts.Repositories;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Queries.GetPatientDetail
{
    public class GetPatientDetailQueryHandler : IRequestHandler<GetPatientDetailQuery, PatientDetailDTO>
    {
        private readonly IPatientRepository repository;

        public GetPatientDetailQueryHandler(IPatientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PatientDetailDTO> Handle(GetPatientDetailQuery request)
        {
            var patient = await repository.GetById(request.Id);
            if (patient == null) {
                throw new DirectoryNotFoundException();
            } 
            return patient.ToDTO();
        }
    }
}
