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
    public class RaportOperacji: Controller
    {       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public RaportOperacji(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.RaportOperacji.GetAll().OrderByDescending(o=> o.Data).Take(500);
            return Json(new { data = allObj });            
        }

        #endregion
    }
}