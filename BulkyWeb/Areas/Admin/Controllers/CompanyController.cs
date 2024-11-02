using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            ViewData["Page"] = pageNumber;
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList(); 
            return View(objCompanyList);
        }
        
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            if(id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            
            if (ModelState.IsValid) 
            {
                if (companyObj.Id == 0)
                {
                    TempData["success"] = "Company created successfully";
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    TempData["success"] = "Company updated successfully";
                    _unitOfWork.Company.Update(companyObj);
                }
                
                _unitOfWork.Save();
                return RedirectToAction("Index", "Company");
            }
            else
            {
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["error"] = "Invalid company id.";
                return Json(new { success = false, message = "Invalid company id." });
            }
            Company? companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                TempData["error"] = "Company not found.";
                return Json(new { success = false, message = "Company not found." });
            }
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            TempData["success"] = "Company deleted successfully";
            return  Json(new { success = true, message = "Company deleted successfully" });
        }

        #region API CAllS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        #endregion
    }
}