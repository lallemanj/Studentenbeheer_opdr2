using System.ComponentModel.DataAnnotations;

namespace Studentenbeheer.Models
{
    public class Gender
    {     
        
       public char ID { get; set; }
       [Required]
       [Display(Name = "Naam")]
       public string Name { get; set; }
    }
}
