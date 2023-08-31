using BlogCore.Data;
using BlogCore.DL.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext dbContext, IWorkContainer workContainer, ApplicationDbContext context)
        {
            //_context = dbContext;
            _workContainer = workContainer;
            _context = context;
        }
        //[AllowAnonymous]
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
            // para trabajar con la tabla se invoca asi
            //var model = _workContainer.Categorie.GetAll();

            //Para utilizar un Stored Procedure  se invoca asi y con el _context, en vez de trabajar con la tabla
            var model = _context.Categories.FromSqlRaw<Category>("Sp_Get_Category").ToList();
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
