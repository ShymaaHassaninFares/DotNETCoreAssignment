using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Product.Domain.Interface;
using Product.Domain.Model;

namespace Product.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private readonly IProductsOp _productRepo;
        private readonly ICategoriesOp _categoryRepo;
        private readonly IWebHostEnvironment _webHost;

        public ProductController(IProductsOp productrepo,ICategoriesOp categoryRepo, IWebHostEnvironment webHost) // here the repository will be passed by the dependency injection.
        {
            _webHost = webHost;
            _productRepo = productrepo;          
            _categoryRepo = categoryRepo;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();

            sortModel.AddColumn("name");
            sortModel.AddColumn("Price");
            sortModel.AddColumn("Quantity");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<Product.Domain.Model.Product> products = _productRepo.GetProducts(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            PopulateViewbags();


            var pager = new PagerModel(products.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(products);
        }


        private void PopulateViewbags()
        {
            ViewBag.Categories = GetCategories();
        }

        public IActionResult Details(int id) //Read
        {
            Product.Domain.Model.Product product = _productRepo.GetProductById(id);
            return View(product);
        }


        public IActionResult Edit(int id)
        {
            Product.Domain.Model.Product returnProduct =  _productRepo.GetProductById(id);
            Product.Domain.Model.UpdateProductModel product = new Product.Domain.Model.UpdateProductModel()
            {
                Id = returnProduct.Id,
                CategoryId = returnProduct.CategoryId,
                ImgURL = returnProduct.ImgURL,
                Name = returnProduct.Name,
                Price = returnProduct.Price,
                ProductPhoto = returnProduct.ProductPhoto,
                Quantity = returnProduct.Quantity,
                Category = returnProduct.Category
            };
               
            ViewBag.Categories = GetCategories();

            TempData.Keep();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product.Domain.Model.UpdateProductModel product)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (product.ProductPhoto != null)
                {
                    string uniqueFileName = GetUploadedFileName(product);
                    product.ImgURL = uniqueFileName;
                }

                if (errMessage == "")
                {
                    _productRepo.UpdateProduct(product);
                    TempData["SuccessMessage"] = product.Name + ", product Saved Successfully";
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }



            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];


            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(product);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            Product.Domain.Model.Product product = _productRepo.GetProductById(id);
            ViewBag.Categories = GetCategories();
            TempData.Keep();
            return View(product);
        }


        [HttpPost]
        public IActionResult Delete(Product.Domain.Model.Product product)
        {
            bool isDeleted = false;
            try
            {
                isDeleted = _productRepo.DeleteProduct(product);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                if (ex.InnerException != null)
                    errMessage = ex.InnerException.Message;

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(product);
            }
            if (isDeleted)
            {
                int currentPage = 1;
                if (TempData["CurrentPage"] != null)
                    currentPage = (int)TempData["CurrentPage"];

                TempData["SuccessMessage"] = "Product " + product.Name + " Deleted Successfully";
                return RedirectToAction(nameof(Index), new { pg = currentPage });
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Product";
                ModelState.AddModelError("", "Failed to Delete Product");
                return View(product);
            }
        }

        private SelectList GetCategories()
        {
            List<Category> items = _categoryRepo.GetCategories();
           
            var defItem = new Category()
            {
                Id = 0,
                Name = "----Select Category----"
            };

            items.Insert(0, defItem);

            return new SelectList(items, nameof(Category.Id), nameof(Category.Name),items[0]);
        }

        private string GetUploadedFileName(Product.Domain.Model.Product product)
        {
            string uniqueFileName = null;

            if (product.ProductPhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ProductPhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.ProductPhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public JsonResult IsProductNameValid(string Name)
        {

            bool isExists = _productRepo.IsItemExists(Name);

            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }

        public JsonResult IsCategoryValid(int CategoryId)
        {

           return CategoryId == 0? Json(data: false) : Json(data: true);
            
        }

    }
}
