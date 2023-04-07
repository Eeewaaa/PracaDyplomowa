using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using RealEstate3.Data;
using RealEstate3.Data.Services;
using RealEstate3.Data.Static;
using RealEstate3.Models;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace RealEstate3.Controllers
{
    [Authorize]

    public class EstatesController : Controller
    {
        private readonly IEstatesService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOwnersService _ownerService;

        public EstatesController(IEstatesService service, IOwnersService ownersService, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _ownerService = ownersService;
        }

        [AllowAnonymous]

        // GET: EstatesControllercs
        public async Task<ActionResult> Index()
        {
            //if (User.IsInRole("Owner"))
            //{
            //    string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //    var result = await _ownerService.IsOwnerAssigned(UserId);
            //    if (result) return RedirectToAction("Create", "Owners");
            //}
            var AllEstates = await _service.GetAllAsync(x => x.PicturesList!, x=> x.Address);
            if(AllEstates == null) return View(NotFound());
            return View(AllEstates);
        }

        [AllowAnonymous]
        
        public async Task<ActionResult> Filter(string searchString)
        {
            var allEstates = await _service.GetAllAsync(x => x.PicturesList!, x => x.Address);
            if (!(string.IsNullOrEmpty(searchString)))
            {
                var filteredResult = allEstates.Where(n => n.Title!.ToLower().Contains(searchString.ToLower()) || n.Description!.ToLower().Contains(searchString.ToLower())).ToList();
                if(filteredResult.Count > 0)
                return View("Index", filteredResult);

                // var filteredResultNew = allEstates.Where(n => string.Equals(n.Name, searchString, Stringcomparison.CurrentCultureIgnoreCase)
                // || string.Equals(n.Description, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

            }
            return View("Index", allEstates);
        }

        [Authorize(Roles = "Admin, Owner, User, TemporaryOwner")]
        // GET: EstatesControllercs/Details/EstateID
        public async Task<IActionResult> Details(int id)
        {
            var estateDetails = await _service.GetEstateByIdAsync(id);
            return View(estateDetails);
        }


        // GET: EstatesControllercs/Create
        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> Create()
        {
            var estateDropdownsData = await _service.GetNewEstateDropdownsValues();
            ViewBag.Owners = new SelectList(estateDropdownsData.Owners, "Id", "Surname");
            ViewBag.Categories = new SelectList(estateDropdownsData.Categories, "Id", "Name");
            return View();
        }


        //POST: EstatesControlercs/AddNewEstate
        [Authorize(Roles = "Admin, Owner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewEstateVM estate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ownerDetails = await _ownerService.GetOwnerByUserId(userId);
            if(!(ownerDetails == null))  estate.OwnerId = ownerDetails.Id;

            if (!ModelState.IsValid)
            {
                var estateDropdownsData = await _service.GetNewEstateDropdownsValues();
                ViewBag.Owners = new SelectList(estateDropdownsData.Owners, "Id", "Surname");
                ViewBag.Categories = new SelectList(estateDropdownsData.Categories, "Id", "Name");
                return View(estate);
            }

            if (estate.PictureFiles != null)
            {
                string folder = "images/estatesGallery/";

                estate.PicturesList = new List<Picture>();

                foreach (var file in estate.PictureFiles)
                {
                    var picture = new Picture()
                    {
                        OriginalName = file.Name,
                        PictureURL = await _service.UploadImage(folder, file)
                    };
                    estate.PicturesList.Add(picture);
                }
            }

            await _service.AddNewEstateAsync(estate);
            if(User.IsInRole("Owner")) return RedirectToAction("ShowMyEstates", "Owners");
            return RedirectToAction("Index");
        }


        //GET: EDIT
        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> Edit(int id)
        {
            var estateDetails = await _service.GetEstateByIdAsync(id);
            if (estateDetails == null) return View("NotFound");
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var OwnerUserId = estateDetails.Owner.UserId;
                if ((OwnerUserId == userId) || User.IsInRole("Admin"))
                {
                    var response = new NewEstateVM()
                    {
                        Id = estateDetails.Id,
                        AvaiableFrom = estateDetails.AvaiableFrom,
                        Description = estateDetails.Description,
                        Balcony = estateDetails.Balcony,
                        Garden = estateDetails.Garden,
                        PetFriendly = estateDetails.PetFriendly,
                        Size = estateDetails.Size,
                        Rent = estateDetails.Rent,
                        Title = estateDetails.Title,
                        CategoryId = estateDetails.CategoryId,
                        OwnerId = estateDetails.OwnerId,
                        Street = estateDetails.Address.Street,
                        Number = estateDetails.Address.Number,
                        PostalCode = estateDetails.Address.PostalCode,
                        PicturesList = estateDetails.PicturesList,
                        PictureFiles = estateDetails.PictureFiles,

                    };
                    var estateDropdownsData = await _service.GetNewEstateDropdownsValues();
                    ViewBag.Owners = new SelectList(estateDropdownsData.Owners, "Id", "Surname");
                    ViewBag.Categories = new SelectList(estateDropdownsData.Categories, "Id", "Name");
                    return View(response);
                } 
                return View("Forbidden");
            }
        }

        // POST: EstatesControllercs/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewEstateVM estate)
        {
           
                if (!ModelState.IsValid)
            {
                var estateDropdownsData = await _service.GetNewEstateDropdownsValues();
                ViewBag.Owners = new SelectList(estateDropdownsData.Owners, "Id", "Surname");
                ViewBag.Categories = new SelectList(estateDropdownsData.Categories, "Id", "Name");
                return View(estate);
            }
            if (User.IsInRole("Owner"))
            {
            var ownerDetails = await _ownerService.GetOwnerByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            estate.OwnerId = ownerDetails.Id;
            }

            await _service.UpdateEstateAsync(estate);
            return RedirectToAction(nameof(Index));

        }

        // GET: OwnerController/Delete/5
        [Authorize(Roles = "Admin, Owner")]
        public async Task<ActionResult> Delete(int id)
        {
            var estateDetails = await _service.GetEstateByIdAsync(id);
            if (estateDetails == null) return View("NotFound");
            if (User.IsInRole("Owner"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var OwnerUserId = estateDetails.Owner.UserId;
                if (OwnerUserId != userId) return View("Forbidden");
                else return View(estateDetails);
            }
            else return View(estateDetails);
        }

        // POST: OwnerController/Delete/5
        [Authorize(Roles = "Admin, Owner")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var estateDetails = await _service.GetEstateByIdAsync(id);
            if (estateDetails == null)
            {
                return View("NotFound");
            }

            if (User.IsInRole("Owner"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var OwnerUserId = estateDetails.Owner.UserId;
                if (OwnerUserId != userId) return View("Forbidden");
                else
                {
                    await _service.DeleteAsync(id);
                    return RedirectToAction("ShowMyEstates", "Owners");
                }
            }
            else
            {
                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
