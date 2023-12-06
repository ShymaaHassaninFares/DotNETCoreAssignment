using Product.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Domain.Interface
{
    public interface IProductsOp
    {
        PaginatedList<Model.Product> GetProducts(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        Model.Product GetProductById(int Id);
        Model.Product GetProductByCategoryId(int Id);
        Model.Product AddProduct(Model.InsertProductModel product);
        Model.Product UpdateProduct(Model.UpdateProductModel product);
        bool DeleteProduct(Model.Product productToBeSaved);
        bool IsItemExists(string name);
    }
}
