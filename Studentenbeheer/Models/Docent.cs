using Studentenbeheer.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentenbeheer.Models
{
    public class Docent
    {
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public DateTime? Deleted { get; set; } = DateTime.MaxValue;

        [ForeignKey("GenderId")]
        public char GeslachtId { get; set; }
        public Gender? Geslacht { get; set; }

        [ForeignKey("AppUserId")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
