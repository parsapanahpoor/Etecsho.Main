using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etecsho.DataAccess.Context;
using Etecsho.Models.Entites.Employee;
using Microsoft.AspNetCore.Authorization;
using Etecsho.DataAccess.Services.Employee;
using Microsoft.AspNetCore.Http;

namespace Etecsho.Main.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly EtecshoContext _context;
        private IEmployeeService _employee;


        public EmployeesController(EtecshoContext context , IEmployeeService employee) 
        {
            _context = context;
            _employee = employee;
        }

        public IActionResult Index(bool Create = false, bool Edit = false, bool Delete = false)
        {
            ViewBag.Create = Create;
            ViewBag.Edit = Edit;
            ViewBag.Delete = Delete;



            return View( _employee.GetAllkEmployees());
        }

        public IActionResult DeletedEmployees()
        {


            return View(_employee.GetDeletedEmployees());
        }


        public IActionResult Details(int? id, bool Delete = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Delete != false)
            {
                ViewData["Delete"] = Delete;
            }


            var employee = _employee.GetEmployeeById((int)id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EmployeeId,UserName,EmployeeLevel,Description,SocialMedia1,SocialMedia2,PersonalLink,UserAvatar,IsDelete")] Employee employee  , IFormFile imgBlogUp)
        {
            if (ModelState.IsValid)
            {
                _employee.AddEmployees(employee, imgBlogUp);


                return Redirect("/Admin/Employees/Index?Create=true");
            }
            return View(employee);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employee.GetEmployeeById((int)id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("EmployeeId,UserName,EmployeeLevel,Description,SocialMedia1,SocialMedia2,PersonalLink,UserAvatar,IsDelete")] Employee employee, IFormFile imgBlogUp) 
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                _employee.UpdateEmployee(employee, imgBlogUp);

                return Redirect("/Admin/Employees/Index?Edit=true");
            }
            return View(employee);
        }

        public IActionResult Delete(int id)
        {
            var employee = _employee.GetEmployeeById(id);
            _employee.DeleteEmployee(employee);
            return Redirect("/Admin/Employees/Index?Delete=true");
        }

    }
}
