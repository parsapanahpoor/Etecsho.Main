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
using Microsoft.AspNetCore.Http;
using Etecsho.DataAccess.Services.Users;

namespace Etecsho.Main.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly EtecshoContext _context;
        private IProductService _product;
        private IUserService _user;

        public ProductsController(EtecshoContext context ,  IProductService product , IUserService user)
        {
            _context = context;
            _product = product;
            _user = user;
        }

        public IActionResult Index(bool Create = false, bool Edit = false, bool Delete = false)
        {
            ViewBag.Create = Create;
            ViewBag.Edit = Edit;
            ViewBag.Delete = Delete;

            return View( _product.GetAllProducts());
        }

      

        public IActionResult Create()
        {
            ViewData["ProductCategories"] = _product.GetAllProductCategories();

            return View();
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductID,UserId,ProductTitle,ShortDescription,LongDescription,ProductImageName,OfferPercent,IsInOffer,ProductCount,Price,Tags,CreateDate,IsActive,IsDelete")] Product product, IFormFile imgProductUp, List<int> SelectedCategory)
        {
            if (ModelState.IsValid)
            {
                var user = _user.GetUserByUserName(User.Identity.Name);
                var ProductID = _product.AddProduct(product, imgProductUp, user);
                _product.AddCategoryToProduct(SelectedCategory, ProductID);


                return Redirect("/Admin/Products/Index?Create=true");

            }
            ViewData["ProductCategories"] = _product.GetAllProductCategories();

            return View(product);
        }

        public IActionResult Edit(int? id , bool Detail = false , bool Delete = false)
        {
            ViewBag.Detail = Detail;
            ViewBag.Delete = Delete;


            if (id == null)
            {
                return NotFound();
            }

            var product = _product.GetProductByID((int)id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCategories"] = _product.GetAllProductCategories();
            ViewData["ProductSelectedCategories"] = _product.GetAllProductSelectedCategories();

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductID,UserId,ProductTitle,ShortDescription,LongDescription,ProductImageName,OfferPercent,IsInOffer,ProductCount,Price,Tags,CreateDate,IsActive,IsDelete")] Product product, IFormFile imgProductUp, List<int> SelectedCategory)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                var productid = _product.UpdateProduct(product, imgProductUp);
                _product.EditProductSelectedCategory(SelectedCategory, productid);

                return Redirect("/Admin/Products/Index?Edit=true");

            }
            ViewData["ProductCategories"] = _product.GetAllProductCategories();
            ViewData["ProductSelectedCategories"] = _product.GetAllProductSelectedCategories();

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _product.GetProductByID(id);
            _product.DeleteProduct(product);
            return Redirect("/Admin/Products/Index?Delete=true");
        }
    }
}
