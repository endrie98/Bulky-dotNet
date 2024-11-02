using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // this below send as index category page and show all the categories
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            // pass the list array of category object in Index View/Category which cames from db
            return View(objCategoryList);
        }

        //this below send us to create category page
        public IActionResult Create()
        {
            return View();
        }

        // this below is the post method for create category
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "The DisplayOrder cannot exactly match the Name.");
            }
            //obj stands for that form in Create.cshtml which is the Category class
            if (ModelState.IsValid) // here ModelSate stands for to valide all requires in Category Model
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                // this only will showed if we came at index page after category object is created in db
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return View();
            }
        }


        // this below takes a single category by id and shows in a edit category page
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        // this below is post method for edit an category
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            //obj stands for that form in Edit.cshtml which is the Category class
            if (ModelState.IsValid) // here ModelSate stands for to valide all requires in Category Model
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                // this only will showed if we came at index page after category object is updated in db
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return View();
            }
        }

        // this below takes a single category by id and shows in a delete category page
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        // this below is delete method for edit an category
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            // this only will showed if we came at index page after category object is delted in db
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index", "Category");
        }
    }
}