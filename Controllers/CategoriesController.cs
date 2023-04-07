using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate3.Data;
using RealEstate3.Data.Services;
using RealEstate3.Data.Static;
using RealEstate3.Models;

namespace RealEstate3.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _service;

        public CategoriesController (ICategoryService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var allCategories = await _service.GetAllAsync(); 
          
            return View(allCategories);
        }

        //GET: categories/ details/id

        public async Task<IActionResult> Details(int id)
        {
            var cateegoriesDetails = await _service.GetByIdAsync(id);
            //if (cateegoriesDetails != null)
            //{
            //    var UpperCategory = await _service.GetByIdAsync((int)cateegoriesDetails.UpperCategoryId);
            //    cateegoriesDetails.UpperCategory = UpperCategory;
            //}
            //if (cateegoriesDetails == null) return View("NotFound");
            

            var subCategories = await _service.GetSubCategories(id);
            ViewBag.SubCategories = subCategories;
            return View(cateegoriesDetails);
        }

        //GET: categories/create
        public async Task<IActionResult> Create()
        {
            ViewBag.list = await _service.SetUpperCategory();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name, UpperCategoryId")] Category category)
        {
            if (!ModelState.IsValid) return View(category);
            if(category.UpperCategoryId == null) return View(category);
            await _service.AddAsync(category);
            return RedirectToAction(nameof(Index));
        }

        //GET: categories/edit
        public async Task<IActionResult> Edit(int id)
        {
            var cateegoriesDetails = await _service.GetByIdAsync(id);
            if (cateegoriesDetails == null) return View("NotFound");
            return View(cateegoriesDetails); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, UpperCategoryId")] Category category)
        {
            if (!ModelState.IsValid) return View(category);

            if (id == category.Id)
            {
                await _service.UpdateAsync(id, category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        //GET: categories/delete
        public async Task<IActionResult> Delete(int id)
        {
            var cateegoriesDetails = await _service.GetByIdAsync(id);
            if (cateegoriesDetails == null) return View("NotFound");
            //var UpperCategory = await _service.GetByIdAsync((int)cateegoriesDetails.UpperCategoryId);
            //cateegoriesDetails.UpperCategory = UpperCategory;
            return View(cateegoriesDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cateegoriesDetails = await _service.GetByIdAsync(id);
            if (cateegoriesDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
