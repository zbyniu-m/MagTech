﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagTech.DataAccess.Repository.IRepository;
using MagTech.Models;
using MagTech.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MagTech.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BazaArtykulowController : Controller
    {       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public BazaArtykulowController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
            if (bazaArtykulow==null)
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
            var allObj = _unitOfWork.BazaArtykulow.GetAll();
            return Json(new { data = allObj });            
        }

        [HttpGet]
        public IActionResult JobHeaderAddOrEdit()
        {
            NaglowekKoszykZadania naglowekZadania = new NaglowekKoszykZadania();
            return PartialView("_NaglowekZadania", naglowekZadania);

        }
        [HttpPost]
        public IActionResult JobHeaderAddOrEdit(NaglowekKoszykZadania naglowekZadania)
        {
            _unitOfWork.NaglowekKoszykZadania.Add(naglowekZadania);
            _unitOfWork.Save();
            var ident =  _unitOfWork.NaglowekKoszykZadania.GetFirstOrDefault(c => c.Nazwa == naglowekZadania.Nazwa && c.Date == naglowekZadania.Date).Id;
            HttpContext.Session.SetObject(SD.ssNaglowekKoszyka, ident);

            return PartialView("_NaglowekZadania", naglowekZadania);

        }

        #endregion
    }
}