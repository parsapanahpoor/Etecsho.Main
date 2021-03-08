using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etecsho.DataAccess.Context;
using Etecsho.Models.Entites.Users;
using Etecsho.DataAccess.Services.Permission;
using Microsoft.AspNetCore.Authorization;
using Etecsho.DataAccess.Security;

namespace Etecsho.Main.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RolesController : Controller
    {
        private IPermissionService _permissionService;

        public RolesController( IPermissionService permission)
        {
            _permissionService = permission;
        }
        [PermissionChecker(2)]

        public IActionResult Index(bool Create = false, bool Edit = false, bool Delete = false)
        {
            var role = _permissionService.GetRoles();
            ViewBag.Create = Create;
            ViewBag.Edit = Edit;
            ViewBag.Delete = Delete;

            return View(role);
        }

        [PermissionChecker(3)]

        public IActionResult Create()
        {
            ViewData["Permissions"] = _permissionService.GetAllPermission();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("RoleId,RoleTitle,IsDelete")] Role role , List<int> SelectedPermission)
        {
            if (ModelState.IsValid)
            {

                role.IsDelete = false;
                int roleId = _permissionService.AddRole(role);

                _permissionService.AddPermissionsToRole(roleId, SelectedPermission);



                return Redirect("/Admin/Roles/Index?Create=true");
            }
            return View(role);
        }
        [PermissionChecker(4)]

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = _permissionService.GetRoleById((int)id);

            if (role == null)
            {
                return NotFound();
            }
            ViewData["Permissions"] = _permissionService.GetAllPermission();
            ViewData["SelectedPermissions"] = _permissionService.PermissionsRole((int)id);
            return View(role);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("RoleId,RoleTitle,IsDelete")] Role role , List<int> SelectedPermission)
        {
            if (id != role.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
            
                    _permissionService.UpdateRole(role);

                    _permissionService.UpdatePermissionsRole(role.RoleId, SelectedPermission);

                
           
                return Redirect("/Admin/Roles/Index?Edit=true");
            }
            return View(role);
        }
        [PermissionChecker(5)]

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Permissions"] = _permissionService.GetAllPermission();
            ViewData["SelectedPermissions"] = _permissionService.PermissionsRole((int)id);
            var role = _permissionService.GetRoleById((int)id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var role = _permissionService.GetRoleById((int)id);

            _permissionService.DeleteRole(role);

            return Redirect("/Admin/Roles/Index?Delete=true");
        }


    }
}
