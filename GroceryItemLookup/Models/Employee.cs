using System.ComponentModel.DataAnnotations;

namespace GroceryItemLookup.Models
{
    public class Employee
    {
        [Key]
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }


        [Required]
        [StringLength(25, ErrorMessage = "First name cannot be longer than 25 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [StringLength(25, ErrorMessage = "Last name cannot be longer than 25 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

    }
}
