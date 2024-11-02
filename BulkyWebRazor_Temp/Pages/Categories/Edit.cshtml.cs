using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category? Category { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? id)
        {
            if(id != null && id != 0)
            {
                Category = _db.Categories.Find(id);
            }
            //if (id == null || id == 0)
            //{
            //    return NotFound();
            //}
            //Category? categoryFromDb = _db.Categories.Find(id);
            //if (categoryFromDb == null)
            //{
            //    return NotFound();
            //}
            //Category = categoryFromDb;
            //return Page();
        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                _db.Categories.Update(Category!);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index", "Categories");
                //Category? categoryFromDb = _db.Categories.Find(Category.Id);
                //if (categoryFromDb == null) 
                //{
                //    return NotFound();
                //}
                //categoryFromDb.Id = Category.Id;
                //categoryFromDb.Name = Category.Name;
                //categoryFromDb.DisplayOrder = Category.DisplayOrder; 
                //_db.Categories.Update(categoryFromDb);
                //_db.SaveChanges();
                //return RedirectToPage("Index", "Categories");
            }
            return Page();
        }
    }
}
