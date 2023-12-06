using Product.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Data
{
    public class ProductContext : IdentityDbContext
    {
        public ProductContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product.Domain.Model.Product> Products { get; set; }


    }
}
