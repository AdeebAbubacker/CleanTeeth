using System.ComponentModel.DataAnnotations;

namespace CleanTeeth.API.DTOs.DentalOffices
{
    public class UpdateDentalOffcieDTO
    {
        [Required]
        [StringLength(50)]
        public required String Name { get; set; }
    }
}
