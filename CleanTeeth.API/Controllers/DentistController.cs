using CleanTeeth.API.DTOs.DentalOffices;
using CleanTeeth.API.DTOs.Dentists;
using CleanTeeth.API.DTOs.Patients;
using CleanTeethApplication.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Commands.DeleteDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Commands.UpdateDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeList;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentistList;
using CleanTeethApplication.Features.Dentists.Commands.CreateDentist;
using CleanTeethApplication.Features.Dentists.Commands.DeleteDentist;
using CleanTeethApplication.Features.Dentists.Commands.UpdateDentist;
using CleanTeethApplication.Features.Dentists.Queries.GetDentistDetails;
using CleanTeethApplication.Features.Dentists.Queries.GetDentistList;
using CleanTeethApplication.Features.Patients.Commands.CreatePatient;
using CleanTeethApplication.Features.Patients.Commands.DeletePatient;
using CleanTeethApplication.Features.Patients.Commands.UpdatePatient;
using CleanTeethApplication.Features.Patients.Queries.GetPatientDetail;
using CleanTeethApplication.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace CleanTeeth.API.Controllers
{
    [ApiController]
    [Route("api/dentist")]
    public class DentistController : ControllerBase
    {
        private readonly IMediator mediator;

        public DentistController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<List<GetDentistListDTO>>> Get()
        {
            var query = new GetDentistListQuery();
            var result = await mediator.Send(query);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDentistDetailDTO>> Get(Guid id)
        {
            var query = new GetDentistDetailQuery { Id = id };
            return await mediator.Send(query);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteDentistCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDentistDTO createDentalOfficeDTO)
        {
            var command = new CreateDentistCommand { Name = createDentalOfficeDTO.Name,Email = createDentalOfficeDTO.Email };
            await mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateDentistDTO updatePatientDTO)
        {
            var command = new UpdateDentistCommand
            {
                Id = id,
                Name = updatePatientDTO.Name,
                email = updatePatientDTO.email
            };
            await mediator.Send(command);
            return NoContent();
        }
    }
}