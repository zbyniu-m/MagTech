using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagTech.DataAccess.Repository.IRepository;
using MagTech.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MagTech.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StanyMinimalneController : Controller
    {       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public StanyMinimalneController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit(int? id)
        {
            BazaArtykulow bazaArtykulow = new BazaArtykulow();
            bazaArtykulow = _unitOfWork.BazaArtykulow.Get(id.GetValueOrDefault());
            if (bazaArtykulow == null)
            {
                return NotFound();
            }
            return View(bazaArtykulow);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(BazaArtykulow bazaArtykulow)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.BazaArtykulow.Update(bazaArtykulow);

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(bazaArtykulow);
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.BazaArtykulow.GetAll().Where(w=> w.StanMinimalny >= w.StanWMiejscuSkladowania && w.StanMinimalny != 0);
            return Json(new { data = allObj });            
        }

        #endregion
    }
}