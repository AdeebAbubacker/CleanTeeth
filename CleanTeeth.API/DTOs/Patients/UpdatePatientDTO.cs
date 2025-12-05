using System.ComponentModel.DataAnnotations;

namespace CleanTeeth.API.DTOs.Patients
{
    public class UpdatePatientDTO
    {
        [Required]
        [StringLength(100)]
        public required String Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public required String Email { get; set; }
    }
}
