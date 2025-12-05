using System.ComponentModel.DataAnnotations;

namespace CleanTeeth.API.DTOs.DentalOffices
{
    public class CreateDentalOfficeDTO
    {
        [Required]
        [StringLength(100)]
        public required String Name { get; set; }
    }
}
