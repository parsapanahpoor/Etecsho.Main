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

        public List<ProductCategories> GetAllProductCategories()
        {
            return _context.ProductCategories.ToList();
        }
    }
}
