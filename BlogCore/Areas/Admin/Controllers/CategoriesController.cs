using BlogCore.Data;
using BlogCore.DL.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IWorkContainer _workContainer;
        //private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext dbContext, IWorkContainer workContainer)
        {
            //_context = dbContext;
            _workContainer = workContainer;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                _workContainer.Categorie.Add(category);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category  category = new Category();
            category = _workContainer.Categorie.Get(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _workContainer.Categorie.Update(category);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);

        }
        #region
        //Call Api
        [HttpGet]
        public IActionResult GetAll() {
            var model = _workContainer.Categorie.GetAll();
            return Json(new
            {
                Result = "Ok",
                Record = model,
                RecordCount = model.Count()
            });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var category = _workContainer.Categorie.Get(id);
            if (category == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error borrando categoría",
                    Result = "Ok"
                });
            }
            _workContainer.Categorie.Remove(category);
            _workContainer.Save();

            return Json(new
            {
                success = true,
                message = "Categoría eliminada correctamente!"
            });
        }

        #endregion
    }
}
