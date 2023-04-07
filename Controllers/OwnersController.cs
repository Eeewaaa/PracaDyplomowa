using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate3.Data;
using RealEstate3.Data.Services;
using RealEstate3.Data.Static;
using RealEstate3.Migrations;
using RealEstate3.Models;
using System.Security.Claims;

namespace RealEstate3.Controllers
{
    [Authorize]
    [Authorize(Roles = "Admin, Owner, User, TemporaryOwner")]
    public class OwnersController : Controller
    {
        private readonly IOwnersService _service;
        private readonly IEstatesService _estateService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OwnersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOwnersService service, IEstatesService estatesService)
        {
            _service = service;
            _estateService = estatesService;
            _userManager = userManager;
            _signInManager = signInManager;

        
        }

        // GET: OwnerController

        public async Task<ActionResult> Index()
        {
            var AllOwners = await _service.GetAllAsync();
            return View(AllOwners);
        }

        // GET: OwnerController/Details/

        public async Task<ActionResult> Details(int id)
        {
            var ownerDetails = await _service.GetByIdAsync(id);
            if (ownerDetails == null) return View("NotFound"); 
            return View(ownerDetails);
        }

        // GET: OwnerController/Create 
        [Authorize(Roles = "Admin, TemporaryOwner")]
        public async Task<IActionResult> Create()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result =  await _service.IsOwnerAssigned(UserId); //czy w bazie nie ma właściciela?
            if(result) return View();
            else return RedirectToAction("Index", "Estates");
        }

        // POST: OwnerController/Create
        [Authorize(Roles = "Admin, TemporaryOwner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.FindByIdAsync(userId); //Maja
            await _userManager.RemoveFromRoleAsync(user, UserRoles.TemporaryOwner);
            await _userManager.AddToRoleAsync(user, "Owner"); //Maja
            await _signInManager.SignInAsync(user, true); //Maja

            if (owner.ProfilePic != null)
            {
                string folder = "images/ownersGallery/";
                owner.ProfilePictureUrl = await _service.UploadImage(folder, owner.ProfilePic);
            }
            owner.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _service.AddAsync(owner);
            return RedirectToAction(nameof(Index));
        }



        // GET: OwnerController/Edit/5 DLA ADMINA i OWNER'a
        [Authorize(Roles = "Admin, Owner")]
        public async Task <ActionResult> Edit(int id)
        {
            var ownerDetails = await _service.GetByIdAsync(id);
            if (ownerDetails == null) return View("NotFound");

            if (User.IsInRole("Owner"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAllowed = _service.ValidateOwner(userId, ownerDetails);
                if (isAllowed)
                {
                    return View(ownerDetails);
                }
                else return View("Forbidden");
            }
            else return View(ownerDetails);
        }

        //EDIT DLA OWNERA
        [Authorize(Roles = "Admin, Owner")]
        public async Task<ActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var profileDetails = await _service.GetOwnerByUserId(userId);
           
            if(profileDetails == null) return View("NotFound");
            
            return RedirectToAction("Edit", new { id = profileDetails.Id } );
        }

        // POST: OwnerController/Edit/
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Owner")]
        public async Task <ActionResult> Edit(int id, Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return View(owner);
            }

            await _service.UpdateAsync(id, owner);
            return RedirectToAction(nameof(Index));
        }




        // GET: OwnerController/Delete/5
        [Authorize(Roles = "Admin, Owner")]
        public async Task <ActionResult> Delete(int id)
        {
            var ownerDetails = await _service.GetByIdAsync(id);
            if (ownerDetails == null) return View("NotFound");
            if (User.IsInRole("Owner"))
            { 
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAllowed = _service.ValidateOwner(userId, ownerDetails);
                if (isAllowed)
                {
                    return View(ownerDetails);
                }
                else return View("NotFound");
            } else return View(ownerDetails);
        }

        // POST: OwnerController/Delete/5
        [Authorize(Roles = "Admin, Owner")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> DeleteConfirmed(int id) { 
            var ownerDetails = await _service.GetByIdAsync(id);
            if (ownerDetails == null)
            {
                return View("NotFound");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //znajdz listę nieruchomosci należącą do tego użytkownika 
            var owner = await _service.GetOwnerByUserId(userId);
            var thisOwnerEstateList = owner.EstateList;

            if (User.IsInRole("Owner"))
            {
                var isAllowed = _service.ValidateOwner(userId, ownerDetails);
                if (isAllowed)
                {
                    // dla każdej z tych nieruchomosci wykonaj operację usuń
                    if(thisOwnerEstateList != null)
                    {
                        foreach (var item in thisOwnerEstateList.ToList())
                        {
                           await _estateService.DeleteAsync(item.Id);
                        }

                    }
                                      
                    //usuń użytkownika
                    await _service.DeleteAsync(id);
                    ApplicationUser user =await _userManager.FindByIdAsync(userId); //Maja
                    await _userManager.RemoveFromRoleAsync(user, UserRoles.Owner);
                    await _userManager.AddToRoleAsync(user, UserRoles.TemporaryOwner); //Maja
                    await _signInManager.SignInAsync(user, true); //Maja
                    return RedirectToAction(nameof(Index));
                }
                else return View("Forbidden");
            }
            else
            {
                // dla każdej z tych nieruchomosci wykonaj operację usuń
                if (thisOwnerEstateList != null)
                {
                    foreach (var item in thisOwnerEstateList.ToList())
                    {
                        await _estateService.DeleteAsync(item.Id);
                    }

                }
                await _service.DeleteAsync(id);
                ApplicationUser user = await _userManager.FindByIdAsync(userId); //Maja
                await _userManager.RemoveFromRoleAsync(user, UserRoles.Owner);
                await _userManager.AddToRoleAsync(user, UserRoles.TemporaryOwner); //Maja
                await _signInManager.SignInAsync(user, true); //Maja

                return RedirectToAction(nameof(Index));
            }

        }

        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> ShowMyEstates()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var thisOwner = await _service.GetOwnerByUserId(userId);
            var allEstates = await _estateService.GetAllAsync(x => x.PicturesList!, x => x.Address);
            var myEstates = allEstates.Where(n => n.OwnerId == thisOwner.Id);
            
            if (myEstates.Any()) return View("~/Views/Estates/Index.cshtml", myEstates);
            else return View ("~/Views/Estates/NoEstates.cshtml");
        }

        public async Task<ActionResult> ShowTheirEstates(int id)
        {
            var thisOwner = await _service.GetByIdAsync(id);
            if (thisOwner == null) return View("NotFound");
            var allEstates = await _estateService.GetAllAsync(x => x.PicturesList!, x => x.Address);
            var theirEstates = allEstates.Where(n => n.OwnerId == thisOwner.Id);
            if (theirEstates.Any()) return View("~/Views/Estates/Index.cshtml", theirEstates);
            else return View("~/Views/Estates/NoEstates.cshtml");

        }

    }
}
