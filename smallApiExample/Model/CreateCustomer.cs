using System.ComponentModel.DataAnnotations;

namespace smallApiExample.Model
{
    public class CreateCustomer
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? FirstName { get; init; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? SurName { get; init; }
    }
}
