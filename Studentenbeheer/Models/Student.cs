using Microsoft.AspNetCore.Mvc.Rendering;
using Studentenbeheer.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentenbeheer.Models
{
    public class Student
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Voornaam")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Achternaam")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Geboortedatum")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public DateTime? Deleted { get; set; } = DateTime.MaxValue;

        [Display(Name = "Geslacht")]
        [ForeignKey("Gender")]
        public char GenderID { get; set; }
        public Gender? Gender { get; set; }

        [Display(Name = "Inschrijvingen")]
        public List<Inschrijvingen>? InschrijvingenLijst { get; set; }

        [ForeignKey("AppUser")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
    public class StudentIndexViewModel
    {
        public int SelectedItem { get; set; }
        public string SearchField { get; set; }
        public List<Student> Students { get; set; }
        public SelectList GendersToSelect { get; set; }


    }

}
