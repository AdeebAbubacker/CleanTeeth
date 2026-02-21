using CleanTeeth.API.DTOs.Patients;
using CleanTeeth.API.Utilities;
using CleanTeethApplication.Common.Response;
using CleanTeethApplication.Features.DentalOffices.Commands.DeleteDentalOffice;
using CleanTeethApplication.Features.Patients.Commands.CreatePatient;
using CleanTeethApplication.Features.Patients.Commands.DeletePatient;
using CleanTeethApplication.Features.Patients.Commands.UpdatePatient;
using CleanTeethApplication.Features.Patients.Queries.GetPatientDetail;
using CleanTeethApplication.Features.Patients.Queries.GetPatientsList;
using CleanTeethApplication.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers
{
    /// <summary>
    /// API endpoints for managing patients
    /// </summary>
    [ApiController]
    [Route("api/patients")]
    [Authorize(Roles = "Dentist")]
    public class PatientsController : BaseController
    {
        private readonly IMediator _mediator;

        public PatientsController(IMediator mediator, ILogger<PatientsController> logger) : base(logger)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all patients with pagination support
        /// </summary>
        /// <param name="query">Pagination query parameters</param>
        /// <returns>List of patients</returns>
        /// <response code="200">Successfully retrieved patients</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PatientListDTO>>>> Get([FromQuery] GetPatientListQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                HttpContext.InsertPaginationInformationHeader(result.TotalAMountOfRecords);
                
                _logger.LogInformation("Retrieved {Count} patients", result.Elements.Count);
                return SuccessResponse<List<PatientListDTO>>(
                    result.Elements, 
                    "Patients retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patients");
                return InternalErrorResponse("An error occurred while retrieving patients",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Get a specific patient by ID
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <returns>Patient details</returns>
        /// <response code="200">Successfully retrieved patient</response>
        /// <response code="404">Patient not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PatientDetailDTO>>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid patient ID");
            }

            var query = new GetPatientDetailQuery { Id = id };
            return await HandleQueryAsync(_mediator, query, "Patient retrieved successfully");
        }

        /// <summary>
        /// Create a new patient
        /// </summary>
        /// <param name="createPatientDTO">Patient creation details</param>
        /// <returns>Created patient</returns>
        /// <response code="201">Successfully created patient</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Post([FromBody] CreateDentistDTO createPatientDTO)
        {
            if (string.IsNullOrWhiteSpace(createPatientDTO.Name) || string.IsNullOrWhiteSpace(createPatientDTO.Email))
            {
                return BadRequestResponse("Name and Email are required");
            }

            var command = new CreatePatientCommand
            {
                Name = createPatientDTO.Name,
                Email = createPatientDTO.Email,
            };
            return await HandleCreatedCommandAsync(_mediator, command, "Patient created successfully");
        }

        /// <summary>
        /// Update a patient
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <param name="updatePatientDTO">Updated patient details</param>
        /// <returns>No content</returns>
        /// <response code="204">Successfully updated patient</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Patient not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdatePatientDTO updatePatientDTO)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid patient ID");
            }

            if (string.IsNullOrWhiteSpace(updatePatientDTO.Name) || string.IsNullOrWhiteSpace(updatePatientDTO.Email))
            {
                return BadRequestResponse("Name and Email are required");
            }

            var command = new UpdatePatientCommand
            {
                Id = id,
                Name = updatePatientDTO.Name,
                Email = updatePatientDTO.Email,
            };
            return await HandleCommandAsync(_mediator, command, "Patient updated successfully");
        }

        /// <summary>
        /// Delete a patient
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Successfully deleted patient</response>
        /// <response code="404">Patient not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid patient ID");
            }

            var command = new DeletePatientCommand { Id = id };
            return await HandleCommandAsync(_mediator, command, "Patient deleted successfully");
        }
    }
}
