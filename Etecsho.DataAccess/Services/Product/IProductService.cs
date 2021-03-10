using Etecsho.Models.Entites.Product;
using Etecsho.Models.Entites.Users;
using Microsoft.AspNetCore.Http;
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

        ProductCategories GetProductCatgeoriesById(int id);

        void UpdateProductCategories(ProductCategories productCategories , int id );

        void DeleteProductCategories(int id );

        #endregion

        #region Product

        List<Models.Entites.Product.Product> GetAllProducts();

        int AddProduct(Models.Entites.Product.Product product, IFormFile imgProductUp, User user);

        void AddCategoryToProduct(List<int> Categories, int ProductID);

        Models.Entites.Product.Product GetProductByID(int productid);

        List<ProductSelectedCategory> GetAllProductSelectedCategories();

        int UpdateProduct(Models.Entites.Product.Product product, IFormFile imgProductUp);

        void EditProductSelectedCategory(List<int> Categories, int productid);


        #endregion
    }
}
