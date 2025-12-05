using CleanTeeth.Application.Contracts.Repositories;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Utilities;
using CleanTeethApplication.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Patients.Queries.GetPatientsList
{
    public class GetPatientListQUeryHandler : IRequestHandler<GetPatientListQuery, PaginatedDTO<PatientListDTO>>
    {
        private readonly IPatientRepository patientRepository;

        public GetPatientListQUeryHandler(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }
        public async Task<PaginatedDTO<PatientListDTO>> Handle(GetPatientListQuery request)
        {
            var patients = await patientRepository.GetFiltered(request);
            var totalAmountOfRecords = await patientRepository.GetTotalAmpountofRecords();
            var patientsDTO = patients.Select(patients => patients.ToDto()).ToList();
            var paginatedDTO = new PaginatedDTO<PatientListDTO>
            {
                Elements = patientsDTO,
                TotalAMountOfRecords = totalAmountOfRecords,
            };
            return paginatedDTO;
        }
    }
}
