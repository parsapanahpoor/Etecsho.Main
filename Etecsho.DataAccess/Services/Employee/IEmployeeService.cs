using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etecsho.DataAccess.Services.Employee
{
     public   interface IEmployeeService
    {

        #region EmployeesPanelAdmin

        void AddEmployees(Models.Entites.Employee.Employee employee, IFormFile imgBlogUp);
        void UpdateEmployee(Models.Entites.Employee.Employee employee, IFormFile imgBlogUp);

        void DeleteEmployee(Models.Entites.Employee.Employee employee);

        List<Etecsho.Models.Entites.Employee.Employee> GetDeletedEmployees();

        List<Models.Entites.Employee.Employee> GetAllkEmployees();

        Models.Entites.Employee.Employee GetEmployeeById(int id);

        #endregion

        #region EmployeeForFrontEnd

        List<Models.Entites.Employee.Employee> GetEmployeeForFrontEnd();

        #endregion

    }
}
