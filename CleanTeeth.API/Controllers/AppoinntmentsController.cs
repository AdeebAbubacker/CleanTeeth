using CleanTeeth.API.DTOs.Appointments;
using CleanTeethApplication.Common.Response;
using CleanTeethApplication.Features.Appointments.Commands.CancelAppointment;
using CleanTeethApplication.Features.Appointments.Commands.CompleteAPpointment;
using CleanTeethApplication.Features.Appointments.Commands.CreateAppointment;
using CleanTeethApplication.Features.Appointments.Queries.GetAppointmentDetail;
using CleanTeethApplication.Features.Appointments.Queries.GetAppointmentList;
using CleanTeethApplication.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers
{
    /// <summary>
    /// API endpoints for managing appointments
    /// </summary>
    [ApiController]
    [Route("api/appointments")]
    [Authorize]
    public class AppoinntmentsController : BaseController
    {
        private readonly IMediator _mediator;

        public AppoinntmentsController(IMediator mediator, ILogger<AppoinntmentsController> logger) : base(logger)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all appointments with pagination support
        /// </summary>
        /// <param name="query">Pagination query parameters</param>
        /// <returns>List of appointments</returns>
        /// <response code="200">Successfully retrieved appointments</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public Task<ActionResult<ApiResponse<List<AppointmentListDTO>>>> Get([FromQuery] GetAppointmentsListQuery query)
        {
            return HandleQueryAsync(_mediator, query, "Appointments retrieved successfully");
        }

        /// <summary>
        /// Get a specific appointment by ID
        /// </summary>
        /// <param name="id">Appointment ID</param>
        /// <returns>Appointment details</returns>
        /// <response code="200">Successfully retrieved appointment</response>
        /// <response code="404">Appointment not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AppointmentDetailDTO>>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid appointment ID");
            }

            var query = new GetAppointmentDetailQuery { Id = id };
            return await HandleQueryAsync(_mediator, query, "Appointment retrieved successfully");
        }

        /// <summary>
        /// Create a new appointment
        /// </summary>
        /// <param name="createAppointmentDTO">Appointment creation details</param>
        /// <returns>Created appointment</returns>
        /// <response code="201">Successfully created appointment</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Post([FromBody] CreateAppointmentDTO createAppointmentDTO)
        {
            if (createAppointmentDTO.PatientId == Guid.Empty || 
                createAppointmentDTO.DentistId == Guid.Empty || 
                createAppointmentDTO.DentalOfficeId == Guid.Empty)
            {
                return BadRequestResponse("Patient ID, Dentist ID, and Dental Office ID are required");
            }

            if (createAppointmentDTO.StartDate >= createAppointmentDTO.EndDate)
            {
                return BadRequestResponse("Start date must be before end date");
            }

            var command = new CreateAppointmentCommand
            {
                patientId = createAppointmentDTO.PatientId,
                DentistId = createAppointmentDTO.DentistId,
                DentalOfficeId = createAppointmentDTO.DentalOfficeId,
                StartDate = createAppointmentDTO.StartDate,
                EndDate = createAppointmentDTO.EndDate
            };
            return await HandleCreatedCommandAsync(_mediator, command, "Appointment created successfully");
        }

        /// <summary>
        /// Mark an appointment as completed
        /// </summary>
        /// <param name="id">Appointment ID</param>
        /// <returns>No content</returns>
        /// <response code="200">Successfully completed appointment</response>
        /// <response code="404">Appointment not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("{id}/completed")]
        public async Task<ActionResult> Complete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid appointment ID");
            }

            var command = new CompleteAppointmentCommand { Id = id };
            return await HandleCommandAsync(_mediator, command, "Appointment marked as completed successfully");
        }

        /// <summary>
        /// Cancel an appointment
        /// </summary>
        /// <param name="id">Appointment ID</param>
        /// <returns>No content</returns>
        /// <response code="200">Successfully cancelled appointment</response>
        /// <response code="404">Appointment not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> Cancel(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid appointment ID");
            }

            var command = new CancelAppointmentCommand { Id = id };
            return await HandleCommandAsync(_mediator, command, "Appointment cancelled successfully");
        }
    }
}
