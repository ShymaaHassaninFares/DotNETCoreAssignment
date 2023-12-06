using Product.Domain.Interface;
using Product.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product.Domain.Handlers
{
    public class CategoriesOp : ICategoriesOp
    {
        #region Variables  
        private readonly IRepository<Category> _categoryModelRepository;
        #endregion
        public CategoriesOp(IRepository<Category> categoryModelRepository)
        {
            _categoryModelRepository = categoryModelRepository;
        }
        List<Category> ICategoriesOp.GetCategories()
        {
            return _categoryModelRepository.GetAll().Select(_ => new Category() { Id = _.Id, Name = _.Name }).Distinct().OrderBy(s => s.Name).ToList(); 
        }
    }
}
