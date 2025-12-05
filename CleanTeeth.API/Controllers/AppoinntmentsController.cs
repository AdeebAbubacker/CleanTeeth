using CleanTeeth.API.DTOs.Appointments;
using CleanTeethApplication.Features.Appointments.Commands.CancelAppointment;
using CleanTeethApplication.Features.Appointments.Commands.CompleteAPpointment;
using CleanTeethApplication.Features.Appointments.Commands.CreateAppointment;
using CleanTeethApplication.Features.Appointments.Queries.GetAppointmentDetail;
using CleanTeethApplication.Features.Appointments.Queries.GetAppointmentList;
using CleanTeethApplication.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppoinntmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AppoinntmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppointmentListDTO>>> Get([FromQuery] GetAppointmentsListQuery query)
        {
            return await mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDetailDTO>> Get(Guid id)
        {
            var query = new GetAppointmentDetailQuery { Id = id };
            return await mediator.Send(query);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateAppointmentDTO createAppointmentDTO)
        {
            var command = new CreateAppointmentCommand
            {
                patientId = createAppointmentDTO.PatientId,
                DentistId = createAppointmentDTO.DentistId,
                DentalOfficeId = createAppointmentDTO.DentalOfficeId,
                StartDate = createAppointmentDTO.StartDate,
                EndDate = createAppointmentDTO.EndDate
            };
            var result = await mediator.Send(command);
            return Ok();
        }

        [HttpPost("{id}/completed")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var command = new CompleteAppointmentCommand { Id = id };
            await mediator.Send(command);
            return Ok();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var command = new CancelAppointmentCommand { Id = id };
            await mediator.Send(command);
            return Ok();
        }
    }
}
