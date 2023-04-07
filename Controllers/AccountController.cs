using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using RealEstate3.Data;
using RealEstate3.Data.Static;
using RealEstate3.Data.ViewModels;
using RealEstate3.Models;
using System.Security.Claims;

namespace RealEstate3.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly REDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, /*RoleManager<ApplicationUser> roleManager,*/ REDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_roleManager = roleManager;
            _context = context;

        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
        public IActionResult Login() => View(new LoginVM());
 
        [HttpPost]

        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if(passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, true, false);
                    var userRole = await _userManager.GetRolesAsync(user);

                    if (result.Succeeded)
                    {
                        
                        if (userRole[0] == "TemporaryOwner") //Maja
                        {
                            return RedirectToAction("Create", "Owners");        
                        }
                        else return RedirectToAction("Index", "Estates");
                    }
                }
                TempData["Error"] = "Błędny login lub hasło, proszę spróbować ponownie";
                return View(loginVM);
            }
            TempData["Error"] = "Błędny login lub hasło, proszę spróbować ponownie";
            return View(loginVM);
        }

        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if(!ModelState.IsValid) return View(registerVM); 

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if(user != null )
            {
                TempData["Error"] = "Ten adres email już jest zarejestrowany";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                UserName = registerVM.UserName,
                Email = registerVM.EmailAddress,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                if (registerVM.Role == Data.Enums.Role.Najemca)
                {            
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return View("RegisterCompleted");
                }

                await _userManager.AddToRoleAsync(newUser, UserRoles.TemporaryOwner);//Maja
                return View("RegisterCompleted");
            }
            return View(registerVM);
        }

        [HttpPost]

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Estates");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
          return View();
        }

        
    }


}
