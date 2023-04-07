using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate3.Data;

namespace RealEstate3.Controllers
{
    public class AdressesController : Controller
    {

        private readonly REDbContext _context;

        public AdressesController( REDbContext context)
        {
            _context = context;
        }
        // GET: AdressesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdressesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdressesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdressesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdressesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdressesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdressesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdressesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
