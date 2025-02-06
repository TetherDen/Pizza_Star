using Lesson_22_Pizza_Star.Models.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Star.Interfaces;
using Pizza_Star.Models;
using Pizza_Star.VIewModel;

namespace Pizza_Star.Controllers
{


    public class CategoryController : Controller
    {
        private readonly ICategory _category;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(ICategory category, IWebHostEnvironment environment)
        {
            _category = category;
            _environment = environment;
        }


        [HttpGet]
        public async Task<IActionResult> Categories(QueryOptions options, string? search)
        {
            IQueryable<Category> categories = _category.GetAll();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                categories = categories.Where(c => c.Name.ToLower().Contains(search));
            }

            var pagedList = new PagedList<Category>(categories, options);
            return View(pagedList);
        }


        [Authorize(Roles = "Admin, Editor")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryViewModel());
        }


        [Authorize(Roles = "Admin, Editor")]
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = model.Name,
                    Description = model.Description,
                    DateOfPublication = DateTime.Now
                };

                if (model.ImageFile != null)
                {
                    category.Image = await SaveImageAsync(model.ImageFile);
                }


                await _category.AddAsync(category);
                return RedirectToAction(nameof(Categories));
            }

            return View(model);
        }


        [Authorize(Roles = "Admin, Editor")]
        //[ValidateAntiForgeryToken]    // <<-- TODO надо фикс в представлении где JS в модальном окне? или где не помню уже....
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _category.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(category.Image))
            {
                var fullPath = Path.Combine(_environment.WebRootPath, category.Image);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            var result = await _category.DeleteAsync(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }


        [Authorize(Roles = "Admin, Editor")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _category.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryViewModel
            {
                Id = category.Id ?? 0, // тк Id Category может быть null ?
                Name = category.Name,
                Description = category.Description,
                ExistingImage = string.IsNullOrEmpty(category.Image) ? null : $"/{category.Image}"
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = await _category.GetByIdAsync(model.Id);
                if (category == null)
                {
                    return NotFound();
                }

                category.Name = model.Name;
                category.Description = model.Description;

                if (model.ImageFile != null)
                {
                    // удаляем старое изображение перед загрузкой нового
                    await DeleteOldImageAsync(category.Image);
                    category.Image = await SaveImageAsync(model.ImageFile);
                }

                await _category.EditAsync(category);
                return RedirectToAction(nameof(Categories));
            }
            return View(model);
        }



        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "categories");

            Directory.CreateDirectory(uploadsFolder);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return Path.Combine("uploads", "categories", uniqueFileName).Replace("\\", "/");
        }

        private async Task DeleteOldImageAsync(string? existingImagePath)
        {
            if (string.IsNullOrEmpty(existingImagePath)) return;

            var fullPath = Path.Combine(_environment.WebRootPath, existingImagePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

    }
}
