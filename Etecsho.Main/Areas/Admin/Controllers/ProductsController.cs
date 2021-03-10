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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.product
                .Include(p => p.Users)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", product.UserId);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,UserId,ProductTitle,ShortDescription,LongDescription,ProductImageName,OfferPercent,IsInOffer,ProductCount,Price,Tags,CreateDate,IsActive,IsDelete")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", product.UserId);
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.product
                .Include(p => p.Users)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.product.FindAsync(id);
            _context.product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.product.Any(e => e.ProductID == id);
        }
    }
}
