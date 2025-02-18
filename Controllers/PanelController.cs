using Pizza_Star.Models;
using Pizza_Star.Models.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Pizza_Star.Interfaces;
using Pizza_Star.Models;
using Pizza_Star.Repository;
using Pizza_Star.ViewModel;

namespace Lesson_22_Pizza_Star.Controllers
{


    [Authorize(Roles = "Admin,Editor")]
    public class PanelController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICategory _category;

        public PanelController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICategory category)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _category = category;
        }

        [Route("/panel")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        //   *** Users - CRUD   ***

        [Authorize(Roles = "Admin")]
        [Route("/panel/users")]
        [HttpGet]
        public IActionResult Users(QueryOptions options)
        {
            var pagedList = new PagedList<User>(_userManager.Users, options);
            return View(pagedList);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public  IActionResult CreateUser()
        {
            return View(new CreateUserViewModel());
        }

        // CREATE User
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "A user with this email already exists.");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Year = model.Year,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        // DELETE
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                return Ok();
            }
            return NotFound();
        }

        // EDIT
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Year = user.Year
            };

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditUser(string userId, EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Year = model.Year;

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Users");
            }

            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }


        // ROLE Edit
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new EditRoleViewModel
            {
                UserId = userId,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Удаляем старые роли
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Error while deleting roles");
                return View(model);
            }

            // если не выбрана ни одна роль, то ставим роль "Client"
            if (model.SelectedRoles == null || !model.SelectedRoles.Any())
            {
                model.SelectedRoles = new[] { "Client" };
            }

            var addResult = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Error while adding roles");
                return View(model);
            }

            return RedirectToAction("Users");
        }

        


        //  *** Эти действия ниже вынести в другие контроллеры и потом убрать ***

        //[Route("/panel/categories")]
        //[HttpGet]
        //public IActionResult Categories()
        //{
        //    return View();
        //}


        //[Route("/panel/categories")]
        //[HttpGet]
        //public async Task<IActionResult> Categories(QueryOptions options, string? search)
        //{
        //    IQueryable<Category> categories = _category.GetAll();

        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        search = search.ToLower();
        //        categories = categories.Where(c => c.Name.ToLower().Contains(search));
        //    }

        //    var pagedList = new PagedList<Category>(categories, options);
        //    return View(pagedList);
        //}



        //[Route("/panel/dishes")]
        //[HttpGet]
        //public IActionResult Dishes()
        //{
        //    return View();
        //}


        [Authorize(Roles = "Admin")]
        [Route("/panel/statistics")]
        [HttpGet]
        public IActionResult Statistics()
        {
            return View();
        }
    }


}
