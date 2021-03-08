using Etecsho.DataAccess.Context;
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

namespace Etecsho.DataAccess.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private EtecshoContext _context;

        public EmployeeService(EtecshoContext context)
        {

            _context = context;

        }




        public void AddEmployees(Models.Entites.Employee.Employee employee, IFormFile imgBlogUp)
        {
            employee.UserAvatar = "no-photo.png";  //تصویر پیشفرض

            if (imgBlogUp != null && imgBlogUp.IsImage())
            {
                employee.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBlogUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", employee.UserAvatar);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBlogUp.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/Thumb", employee.UserAvatar);

                imgResizer.Image_resize(imagePath, thumbPath, 150);


            }

            _context.employee.Add(employee);
            _context.SaveChanges();
        }

        public void DeleteEmployee(Models.Entites.Employee.Employee employee)
        {
            employee.IsDelete = true;
            _context.Update(employee);
            _context.SaveChanges();
        }

        public List<Models.Entites.Employee.Employee> GetAllkEmployees()
        {
            return _context.employee.ToList();
        }

        public List<Models.Entites.Employee.Employee> GetDeletedEmployees()
        {
            return _context.employee.IgnoreQueryFilters().ToList();
        }

        public Models.Entites.Employee.Employee GetEmployeeById(int id)
        {
            return _context.employee.Find(id);
        }

        public List<Models.Entites.Employee.Employee> GetEmployeeForFrontEnd()
        {
            return _context.employee.ToList();
        }

        public void UpdateEmployee(Models.Entites.Employee.Employee employee, IFormFile imgBlogUp)
        {
            if (imgBlogUp != null && imgBlogUp.IsImage())
            {

                if (employee.UserAvatar != "no-photo.png")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", employee.UserAvatar);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/Thumb", employee.UserAvatar);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }



                employee.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBlogUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", employee.UserAvatar);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBlogUp.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/Thumb", employee.UserAvatar);

                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }


            _context.employee.Update(employee);
            _context.SaveChanges();
        }
    }
}
