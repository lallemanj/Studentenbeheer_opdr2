using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentenbeheer.Models
{
    public class Module
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        public DateTime? Deleted { get; set; }  = DateTime.MaxValue;

        [Required]
        [Display(Name = "Omschrijving")]
        public string Description { get; set; }

        [Display(Name = "InschrijvingenID")]
        [NotMapped]
        public List<int> InschrijvingenLijstIds { get; set; }

        [Display(Name = "Inschrijvingen")]
        public List<Inschrijvingen>? InschrijvingenLijst { get; set; }

        public List<DocentModule>? DocentModules { get; set; }


    }
    public class Inschrijvingen
    {

        public int Id { get; set; }


        [Display(Name = "Module")]
        public Module? Module { get; set; }

        [ForeignKey("Module")]
        [Display(Name = "ModuleID")]
        public int ModuleIds { get; set; }

        [Display(Name = "Student")]
        public Student? Student { get; set; }

        [Display(Name = "StudentID")]
        public int StudentIds { get; set; }

        [Required]
        [Display(Name = "Inschrijvingsdatum")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [Display(Name = "Afgelegd op")]
        [DataType(DataType.Date)]
        public DateTime TakenOn { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Resultaat")]
        public int Result { get; set; }
    }

}
