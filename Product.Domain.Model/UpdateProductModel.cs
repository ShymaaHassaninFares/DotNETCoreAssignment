using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Domain.Model
{
    public class UpdateProductModel: Product
    {

        [ForeignKey("Categories")]
        [Display(Name = "Category")]
        [Remote("IsCategoryValid", "Product", AdditionalFields = "Id", ErrorMessage = "Category must be selected")]
        public override int CategoryId { get; set; }

    }
}
