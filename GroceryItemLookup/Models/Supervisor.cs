using System.ComponentModel.DataAnnotations;

namespace GroceryItemLookup.Models
{
    public class Supervisor : Employee
    {
        [Required]
        [Display(Name = "Departments")]
        public List<Department> Departments { get; set; }

    }
}
