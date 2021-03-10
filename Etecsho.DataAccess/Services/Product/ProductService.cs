using Etecsho.DataAccess.Context;
using Etecsho.Models.Entites.Product;
using Etecsho.Models.Entites.Users;
using Etecsho.Utilities.Convertors;
using Etecsho.Utilities.Genarator;
using Etecsho.Utilities.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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

        public void AddCategoryToProduct(List<int> Categories, int ProductID)
        {
            foreach (var item in Categories)
            {
                _context.ProductSelectedCategory.Add(new ProductSelectedCategory()
                {

                    ProductCategoryId = item,
                    ProductID = ProductID

                });

                _context.SaveChanges();

            }
        }

        public int AddProduct(Models.Entites.Product.Product product, IFormFile imgProductUp, User user)
        {

            product.UserId = user.UserId;
            product.IsActive = true;
            product.CreateDate = DateTime.Now;
            product.ProductImageName = "no-photo.png";  //تصویر پیشفرض
            //TODO Check Image
            if (imgProductUp != null && imgProductUp.IsImage())
            {
                product.ProductImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgProductUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/image", product.ProductImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgProductUp.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/thumb", product.ProductImageName);

                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }


            _context.Add(product);
            _context.SaveChanges();

            return product.ProductID;
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

        public void DeleteProductCategories(int id)
        {
            ProductCategories productCategories = GetProductCatgeoriesById(id);
            productCategories.IsDelete = true;
            _context.Update(productCategories);
            _context.SaveChanges();
        }

        public List<ProductCategories> GetAllProductCategories()
        {
            return _context.ProductCategories.ToList();
        }

        public List<Models.Entites.Product.Product> GetAllProducts()
        {
            return _context.product.Include(p=>p.Users).ToList();
        }

        public ProductCategories GetProductCatgeoriesById(int id)
        {
            return _context.ProductCategories.Find(id);
        }

        public void UpdateProductCategories(ProductCategories productCategories, int id)
        {
            ProductCategories category = GetProductCatgeoriesById(id);
            category.CategoryTitle = productCategories.CategoryTitle;
            _context.ProductCategories.Update(category);
            _context.SaveChanges();
        }
    }
}
