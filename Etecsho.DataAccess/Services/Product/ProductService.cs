﻿using Etecsho.DataAccess.Context;
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

        public void AddFeatureToProduct(ProductFeature feature)
        {
            _context.ProductFeature.Add(feature);
            _context.SaveChanges();
        }

        public void AddImageToGalleryProduct(ProductGallery productGallery, IFormFile imgUp)
        {

            productGallery.ImageName = "no-photo.png";  //تصویر پیشفرض
            //TODO Check Image
            if (imgUp != null && imgUp.IsImage())
            {
                productGallery.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/image/ProducGallery", productGallery.ImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgUp.CopyTo(stream);
                }

               
            }


            _context.Add(productGallery);
            _context.SaveChanges();

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

        public void DeleteProduct(Models.Entites.Product.Product product)
        {
            product.IsDelete = true;
            _context.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProductCategories(int id)
        {
            ProductCategories productCategories = GetProductCatgeoriesById(id);
            productCategories.IsDelete = true;
            _context.Update(productCategories);
            _context.SaveChanges();
        }

        public void DeleteProductFeature(ProductFeature feature)
        {
            _context.ProductFeature.Remove(feature);
            _context.SaveChanges();
        }

        public void EditProductSelectedCategory(List<int> Categories, int productid)
        {
            _context.ProductSelectedCategory.Where(p => p.ProductID == productid).ToList()
                                                    .ForEach(p => _context.ProductSelectedCategory.Remove(p));

            AddCategoryToProduct(Categories, productid);
        }

        public List<Models.Entites.Product.Product> GetAllDeletedProducts()
        {
            IQueryable<Models.Entites.Product.Product> result = _context.product.Include(p => p.Users)
                          .IgnoreQueryFilters().Where(u => u.IsDelete);

            return result.ToList();
        }

        public List<ProductCategories> GetAllProductCategories()
        {
            return _context.ProductCategories.ToList();
        }

        public List<Models.Entites.Product.Product> GetAllProducts()
        {
            return _context.product.Include(p=>p.Users).ToList();
        }

        public List<ProductSelectedCategory> GetAllProductSelectedCategories()
        {
            return _context.ProductSelectedCategory.ToList();
        }

        public ProductFeature GetFeatureById(int id)
        {
            return _context.ProductFeature.Find(id);
        }

        public List<ProductGallery> GetGalleryById(int id)
        {
            return _context.ProductGallery.Where(p => p.ProductID == id).ToList();
        }

        public Models.Entites.Product.Product GetProductByID(int productid)
        {
            return _context.product.Include(p => p.Users).FirstOrDefault(p => p.ProductID == productid);
        }

        public ProductCategories GetProductCatgeoriesById(int id)
        {
            return _context.ProductCategories.Find(id);
        }

        public List<ProductFeature> GetProductFeaturs(int id)
        {
            return _context.ProductFeature.Where(p => p.ProductID == id).ToList();
        }

        public int UpdateProduct(Models.Entites.Product.Product product, IFormFile imgProductUp)
        {
            //TODO Check Image
            if (imgProductUp != null && imgProductUp.IsImage())
            {

                if (product.ProductImageName != "no-photo.png")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/image", product.ProductImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/thumb", product.ProductImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }



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


            _context.product.Update(product);
            _context.SaveChanges();

            return product.ProductID;
        }

        public void UpdateProductCategories(ProductCategories productCategories, int id)
        {
            ProductCategories category = GetProductCatgeoriesById(id);
            category.CategoryTitle = productCategories.CategoryTitle;
            _context.ProductCategories.Update(category);
            _context.SaveChanges();
        }

        public void UpdateProductForLock(Models.Entites.Product.Product product)
        {
            _context.Update(product);
            _context.SaveChanges();
        }
    }
}
