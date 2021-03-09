using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etecsho.DataAccess.Context;
using Etecsho.Models.Entites.Product;
using Microsoft.AspNetCore.Authorization;
using Etecsho.DataAccess.Services.Product;

namespace Etecsho.Main.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductCategoriesController : Controller
    {
        private readonly EtecshoContext _context;
        private IProductService _product;

        public ProductCategoriesController(EtecshoContext context , IProductService product)
        {
            _context = context;
            _product = product;
        }

        public IActionResult Index(bool Create = false, bool Edit = false, bool Delete = false)
        {
            ViewBag.Create = Create;
            ViewBag.Edit = Edit;
            ViewBag.Delete = Delete;


            return View( _product.GetAllProductCategories());
        }


        public IActionResult Create(int? id)
        {
            return View(new ProductCategories()
            {

                ParentId = id

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductCategoryId,CategoryTitle,IsDelete,ParentId")] ProductCategories productCategories)
        {
            if (ModelState.IsValid)
            {
                _product.AddProductCategories(productCategories);
                return Redirect("/Admin/ProductCategories/Index?Create=true");
            }
            return View(productCategories);
        }

        public  IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategories = _product.GetProductCatgeoriesById((int)id);
            if (productCategories == null)
            {
                return NotFound();
            }
            return View(productCategories);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductCategoryId,CategoryTitle,IsDelete,ParentId")] ProductCategories productCategories)
        {
            if (id != productCategories.ProductCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                _product.UpdateProductCategories(productCategories, id);


                return Redirect("/Admin/ProductCategories/Index?Edit=true");
            }
            return View(productCategories);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategories = _product.GetProductCatgeoriesById((int)id);
            if (productCategories == null)
            {
                return NotFound();
            }

            return View(productCategories);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

            _product.DeleteProductCategories((int)id);


            return Redirect("/Admin/ProductCategories/Index?Delete=true");
        }

       
    }
}
