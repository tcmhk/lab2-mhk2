using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    public class Employer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Website { get; set; }
        [Display(Name = "Incorporated Date")]
        public DateTime IncorporatedDate { get; set; }
    }
}
