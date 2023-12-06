using AutoMapper;
using Product.Domain.Interface;
using Product.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product.Domain.Handlers
{
    public class ProductsOp : IProductsOp
    {
        #region Variables  
        private readonly IRepository<Model.Product> _productModelRepository;
        private readonly IRepository<Category> _categoryModelRepository;
        #endregion
        public ProductsOp(IRepository<Model.Product> productModelRepository, IRepository<Category> categoryModelRepository)
        {
            _productModelRepository = productModelRepository;
            _categoryModelRepository = categoryModelRepository;
        }
        public PaginatedList<Model.Product> GetProducts(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Model.Product> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _productModelRepository.GetAll().Where(_ => _.Category.Name.Contains(SearchText) || _.Name.Contains(SearchText)).Select(_ => new Product.Domain.Model.Product() { Id = _.Id, Name = _.Name, Category = _.Category, ImgURL = _.ImgURL, Price = _.Price, Quantity = _.Quantity, CategoryId = _.Category.Id }).Distinct().OrderBy(s => s.Name).ToList();
            }
            else
                items =_productModelRepository.GetAll().Select(_ => new Product.Domain.Model.Product() { Id = _.Id, Name = _.Name, Category = _.Category, ImgURL = _.ImgURL, Price = _.Price, Quantity = _.Quantity, CategoryId = _.Category.Id }).Distinct().OrderBy(s => s.Name).ToList();
            items = Operations.DoSort(items, SortProperty, sortOrder);
            return new PaginatedList<Model.Product>(items, pageIndex, pageSize);
        }

        public Model.Product GetProductByCategoryId(int Id)
        {
            var category = _categoryModelRepository.GetAll().Where(_ => _.Id == Id).FirstOrDefault(); 
            if(category == null)
            {
               throw new ArgumentException("Category Id not found");
            }
            var selectProduct = _productModelRepository.GetAll().Where(_ => _.CategoryId == Id).Select(_ => new Product.Domain.Model.Product() { Id = _.Id, Name = _.Name, Category = _.Category, ImgURL = _.ImgURL, Price = _.Price, Quantity = _.Quantity, CategoryId = _.Category.Id }).FirstOrDefault();
            if (selectProduct == null) throw new ArgumentNullException(nameof(selectProduct));
            return selectProduct;
        }

        public Model.Product GetProductById(int Id)
        {
            if (Id < 1) throw new ArgumentException("product Id can't less than 1");

            var selectProduct = _productModelRepository.GetAll().Where(_ => _.Id == Id).Select(_ => new Product.Domain.Model.Product() { Id = _.Id, Name = _.Name, Category = _.Category, ImgURL = _.ImgURL, Price = _.Price, Quantity = _.Quantity, CategoryId = _.Category.Id }).FirstOrDefault();
            if (selectProduct == null) throw new ArgumentNullException(nameof(selectProduct));
            return selectProduct;
        }

        public Model.Product AddProduct(Model.InsertProductModel productToBeSaved)
        {
            if (productToBeSaved == null) throw new ArgumentNullException(nameof(productToBeSaved));
            if (string.IsNullOrEmpty(productToBeSaved.Name)) throw new ArgumentNullException("Product Name is mandatory");
            var category = _categoryModelRepository.GetAll().Where(_ => _.Id == productToBeSaved.CategoryId).FirstOrDefault();
            if (category == null)
            {
                throw new ArgumentException("Category Id not found");
            }
            var createdProduct  = new Model.Product
            {
                Name = productToBeSaved.Name,
                ImgURL = productToBeSaved.ImgURL,
                Price = productToBeSaved.Price,
                Quantity = productToBeSaved.Quantity,
                CategoryId = productToBeSaved.CategoryId
            };
            _productModelRepository.Add(createdProduct);
            _productModelRepository.UnitOfWork.Commit();
            return GetProductById(createdProduct.Id);
        }

        public Model.Product UpdateProduct(Model.UpdateProductModel productToBeSaved)
        {
            if (productToBeSaved == null) throw new ArgumentNullException(nameof(productToBeSaved));
           
            var savedProduct = _productModelRepository.GetAll().Where(_ => _.Id == productToBeSaved.Id).FirstOrDefault();
            if (savedProduct == null)
            {
                throw new ArgumentException("Product is not found");
            }
            var category = _categoryModelRepository.GetAll().Where(_ => _.Id == productToBeSaved.CategoryId).FirstOrDefault();
            if (category == null)
            {
                throw new ArgumentException("Category Id not found");
            }
            if (string.IsNullOrEmpty(productToBeSaved.Name)) throw new ArgumentNullException("Product Name is mandatory");

            savedProduct.ImgURL = productToBeSaved.ImgURL;
            savedProduct.Name = productToBeSaved.Name;
            savedProduct.Price = productToBeSaved.Price;
            savedProduct.Quantity = productToBeSaved.Quantity;
            savedProduct.CategoryId = productToBeSaved.CategoryId;

            _productModelRepository.Update(savedProduct);
            _productModelRepository.UnitOfWork.Commit();
            return GetProductById(savedProduct.Id);
        }

        public bool DeleteProduct(Model.Product productToBeSaved)
        {
            if (productToBeSaved == null) throw new ArgumentNullException(nameof(productToBeSaved));

            var savedProduct = _productModelRepository.GetAll().Where(_ => _.Id == productToBeSaved.Id).FirstOrDefault();
            if (savedProduct == null)
            {
                throw new ArgumentException("Product is not found");
            }


            _productModelRepository.Delete(savedProduct);
            _productModelRepository.UnitOfWork.Commit();
            return true;
        }

        public bool IsItemExists(string name)
        {
            int ct = _productModelRepository.GetAll().Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

    }
}
