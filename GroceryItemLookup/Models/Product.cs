using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GroceryItemLookup.Models
{
    public class Product
    {
        [Key]
        [Required]
        [Display(Name = "SKU")]
        public int SKU { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Product name cannot be longer than 50 characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Weight")]
        public string Weight { get; set; }

    }
}
