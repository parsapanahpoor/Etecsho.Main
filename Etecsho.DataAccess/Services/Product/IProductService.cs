using Etecsho.Models.Entites.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etecsho.DataAccess.Services.Product
{
     public  interface IProductService
    {

        #region ProductCategories

        List<ProductCategories> GetAllProductCategories();

        void AddProductCategories(ProductCategories productCategories);

        #endregion
    }
}
