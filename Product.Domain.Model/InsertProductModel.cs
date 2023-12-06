using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Domain.Model
{
    public class InsertProductModel : Product
    {

        [Remote("IsProductNameValid", "Product", AdditionalFields = "Code", ErrorMessage = "Product Name Exists Already")]
        [Required]
        [StringLength(75)]
        public override string Name { get; set; }

        [ForeignKey("Categories")]
        [Display(Name = "Category")]
        [Remote("IsCategoryValid", "Product", AdditionalFields = "Id", ErrorMessage = "Category must be selected")]
        public override int CategoryId { get; set; }

    }
}

