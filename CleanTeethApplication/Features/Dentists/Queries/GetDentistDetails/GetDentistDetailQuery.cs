using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using CleanTeethApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Features.Dentists.Queries.GetDentistDetails
{
    public class GetDentistDetailQuery : IRequest<GetDentistDetailDTO>
    {
        public required Guid Id { get; set; }
    }
}
