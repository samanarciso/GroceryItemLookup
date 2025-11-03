using System.ComponentModel.DataAnnotations;

namespace GroceryItemLookup.Models
{
    public class Department
    {
        [Key]
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "Product name cannot be longer than 50 characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Products")]
        public List<Product> Products { get; set; }

        [Required]
        [Display(Name = "Supervisors")]
        public List<Employee> Supervisors { get; set; }

    }
}
