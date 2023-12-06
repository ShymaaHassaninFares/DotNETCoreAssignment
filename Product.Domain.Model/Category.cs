using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Product.Domain.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

    }
}
