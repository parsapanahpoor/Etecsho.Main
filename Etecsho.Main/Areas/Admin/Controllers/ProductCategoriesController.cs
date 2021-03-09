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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategories = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategories == null)
            {
                return NotFound();
            }

            return View(productCategories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCategoryId,CategoryTitle,IsDelete,ParentId")] ProductCategories productCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCategories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productCategories);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategories = await _context.ProductCategories.FindAsync(id);
            if (productCategories == null)
            {
                return NotFound();
            }
            return View(productCategories);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductCategoryId,CategoryTitle,IsDelete,ParentId")] ProductCategories productCategories)
        {
            if (id != productCategories.ProductCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCategories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoriesExists(productCategories.ProductCategoryId))
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
            return View(productCategories);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategories = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategories == null)
            {
                return NotFound();
            }

            return View(productCategories);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCategories = await _context.ProductCategories.FindAsync(id);
            _context.ProductCategories.Remove(productCategories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoriesExists(int id)
        {
            return _context.ProductCategories.Any(e => e.ProductCategoryId == id);
        }
    }
}
