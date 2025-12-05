using CleanTeeth.Domain.Entities.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace CleanTeeth.API.DTOs.Dentists
{
    public class UpdateDentistDTO
    {
        [Required]
        [StringLength(50)]
        public required String Name { get; set; }

        public required string email { get; set; }
    }
}

