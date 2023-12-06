using Product.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Domain.Interface
{
    public interface ICategoriesOp
    {
        List<Category> GetCategories();
    }
}
