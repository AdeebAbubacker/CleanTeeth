using CleanTeeth.API.DTOs.DentalOffices;
using CleanTeeth.API.DTOs.Dentists;
using CleanTeeth.API.DTOs.Patients;
using CleanTeethApplication.Common.Response;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers
{
    /// <summary>
    /// API endpoints for managing dentists
    /// </summary>
    [ApiController]
    [Route("api/dentist")]
    [Authorize]
    public class DentistController : BaseController
    {
        private readonly IMediator _mediator;

        public DentistController(IMediator mediator, ILogger<DentistController> logger) : base(logger)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all dentists
        /// </summary>
        [HttpGet]
        public Task<ActionResult<ApiResponse<List<GetDentistListDTO>>>> Get()
        {
            var query = new GetDentistListQuery();
            return HandleQueryAsync(_mediator, query, "Dentists retrieved successfully");
        }

        /// <summary>
        /// Get a specific dentist by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetDentistDetailDTO>>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid dentist ID");
            }

            var query = new GetDentistDetailQuery { Id = id };
            return await HandleQueryAsync(_mediator, query, "Dentist retrieved successfully");
        }

        /// <summary>
        /// Delete a dentist
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid dentist ID");
            }

            var command = new DeleteDentistCommand { Id = id };
            return await HandleCommandAsync(_mediator, command, "Dentist deleted successfully");
        }

        /// <summary>
        /// Create a new dentist
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Post([FromBody] CreateDentistDTO createDentistDTO)
        {
            if (string.IsNullOrWhiteSpace(createDentistDTO.Name))
            {
                return BadRequestResponse("Dentist name is required");
            }

            var command = new CreateDentistCommand
            {
                Name = createDentistDTO.Name,
                Email = createDentistDTO.Email
            };
            return await HandleCreatedCommandAsync(_mediator, command, "Dentist created successfully");
        }

        /// <summary>
        /// Update a dentist
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateDentistDTO updateDentistDTO)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid dentist ID");
            }

            if (string.IsNullOrWhiteSpace(updateDentistDTO.Name))
            {
                return BadRequestResponse("Dentist name is required");
            }

            var command = new UpdateDentistCommand
            {
                Id = id,
                Name = updateDentistDTO.Name,
                email = updateDentistDTO.email
            };
            return await HandleCommandAsync(_mediator, command, "Dentist updated successfully");
        }
    }
}