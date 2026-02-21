using CleanTeeth.API.DTOs.DentalOffices;
using CleanTeethApplication.Common.Response;
using CleanTeethApplication.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Commands.DeleteDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Commands.UpdateDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeList;
using CleanTeethApplication.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers
{
    /// <summary>
    /// API endpoints for managing dental offices
    /// </summary>
    [ApiController]
    [Route("api/dentaloffices")]
    [Authorize]
    public class DentalOfficesController : BaseController
    {
        private readonly IMediator _mediator;

        public DentalOfficesController(IMediator mediator, ILogger<DentalOfficesController> logger) : base(logger)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all dental offices
        /// </summary>
        /// <returns>List of all dental offices</returns>
        /// <response code="200">Successfully retrieved list of dental offices</response>
        /// <response code="401">Unauthorized - JWT token required</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<GetDentalOfficeListDTO>>>> Get()
        {
            var query = new GetDentalOfficeListQuery();
            return await HandleQueryAsync(_mediator, query, "Dental offices retrieved successfully");
        }

        /// <summary>
        /// Get a specific dental office by ID
        /// </summary>
        /// <param name="id">Dental office ID</param>
        /// <returns>Dental office details</returns>
        /// <response code="200">Successfully retrieved dental office</response>
        /// <response code="404">Dental office not found</response>
        /// <response code="401">Unauthorized - JWT token required</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DentalOfficeDetailDTO>>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid dental office ID");
            }

            var query = new GetDentalOfficeDetailQuery { Id = id };
            return await HandleQueryAsync(_mediator, query, "Dental office retrieved successfully");
        }

        /// <summary>
        /// Create a new dental office
        /// </summary>
        /// <param name="createDentalOfficeDTO">Dental office creation details</param>
        /// <returns>Created dental office</returns>
        /// <response code="201">Successfully created dental office</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Unauthorized - JWT token required</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Post([FromBody] CreateDentalOfficeDTO createDentalOfficeDTO)
        {
            if (string.IsNullOrWhiteSpace(createDentalOfficeDTO.Name))
            {
                return BadRequestResponse("Dental office name is required");
            }

            var command = new CreateDentalOfficeCommand { Name = createDentalOfficeDTO.Name };
            return await HandleCreatedCommandAsync(_mediator, command, "Dental office created successfully");
        }

        /// <summary>
        /// Update a dental office
        /// </summary>
        /// <param name="id">Dental office ID</param>
        /// <param name="updateDentalOffcieDTO">Updated dental office details</param>
        /// <returns>No content</returns>
        /// <response code="204">Successfully updated dental office</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Dental office not found</response>
        /// <response code="401">Unauthorized - JWT token required</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateDentalOffcieDTO updateDentalOffcieDTO)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid dental office ID");
            }

            if (string.IsNullOrWhiteSpace(updateDentalOffcieDTO.Name))
            {
                return BadRequest("Dental office name is required");
            }

            var command = new UpdateDentalOfficeCommand
            {
                Id = id,
                Name = updateDentalOffcieDTO.Name
            };
            return await HandleCommandAsync(_mediator, command, "Dental office updated successfully");
        }

        /// <summary>
        /// Delete a dental office
        /// </summary>
        /// <param name="id">Dental office ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Successfully deleted dental office</response>
        /// <response code="404">Dental office not found</response>
        /// <response code="401">Unauthorized - JWT token required</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequestResponse("Invalid dental office ID");
            }

            var command = new DeleteDentalOfficeCommand { Id = id };
            return await HandleCommandAsync(_mediator, command, "Dental office deleted successfully");
        }
    }
}
