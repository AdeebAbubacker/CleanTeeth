using CleanTeeth.API.DTOs.DentalOffices;
using CleanTeethApplication.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Commands.DeleteDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Commands.UpdateDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeList;
using CleanTeethApplication.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers
{
    [ApiController]
    [Route("api/dentaloffices")]
    public class DentalOfficesController : ControllerBase
    {
        private readonly IMediator mediator;

        public DentalOfficesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetDentalOfficeListDTO>>> Get()
        {
            var query = new GetDentalOfficeListQuery();
            var result = await mediator.Send(query);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DentalOfficeDetailDTO>> Get(Guid id)
        {
            var query = new GetDentalOfficeDetailQuery { Id = id };
            var result = await mediator.Send(query);
            return result;
        }


        [HttpPost]
        public async Task<IActionResult> Post(CreateDentalOfficeDTO createDentalOfficeDTO)
        {
            var command = new CreateDentalOfficeCommand { Name = createDentalOfficeDTO.Name };
            await mediator.Send(command);
            return Ok();
        }

        /// No content response means it will not return json
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id,UpdateDentalOffcieDTO updateDentalOffcieDTO)
        {
            var command = new UpdateDentalOfficeCommand
            {
                Id = id,
                Name = updateDentalOffcieDTO.Name
            };
            await mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteDentalOfficeCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}
