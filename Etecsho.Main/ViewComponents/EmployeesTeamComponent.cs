using Etecsho.DataAccess.Services.Employee;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etecsho.Main.ViewComponents
{
    public class EmployeesTeamComponent : ViewComponent
    {
        private IEmployeeService _employee;

        public EmployeesTeamComponent(IEmployeeService employee)
        {
            _employee = employee;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {


            return await Task.FromResult((IViewComponentResult)View("EmployeesTeamComponent" ,_employee.GetEmployeeForFrontEnd()));

        }


    }
}
