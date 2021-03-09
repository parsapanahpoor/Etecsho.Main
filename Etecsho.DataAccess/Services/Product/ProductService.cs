using Etecsho.DataAccess.Context;
using Etecsho.Models.Entites.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Etecsho.DataAccess.Services.Product
{
    public class ProductService : IProductService
    {
        private EtecshoContext _context;
        public ProductService( EtecshoContext context)
        {
            _context = context;
        }

        public void AddProductCategories(ProductCategories productCategories)
        {
            ProductCategories cat = new ProductCategories();
            cat.CategoryTitle = productCategories.CategoryTitle;
            cat.IsDelete = false;
            cat.ParentId = productCategories.ParentId;

            _context.ProductCategories.Add(cat);
            _context.SaveChanges();
        }

        public List<ProductCategories> GetAllProductCategories()
        {
            return _context.ProductCategories.ToList();
        }
    }
}
