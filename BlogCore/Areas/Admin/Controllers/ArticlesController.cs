using BlogCore.Data;
using BlogCore.DL.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticlesController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ArticlesController(IWorkContainer workContainer, IWebHostEnvironment hostEnvironment)
        {

            _workContainer = workContainer;
            _hostEnvironment = hostEnvironment; 
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            ArticleVM article = new ArticleVM()
            {
              Article =  new BlogCore.Models.Article(),
              ListCategories = _workContainer.Categorie.GetCategoryList()

            };
            return View(article);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticleVM articleVM)
        {
            if (ModelState.IsValid)
            {
                string mainPatch = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(articleVM.Article.Id == 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string articlePatch = @"images\articles";
                    var dir = Path.GetDirectoryName(mainPatch + @"\" + articlePatch + @"\");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    var uploads = Path.Combine(mainPatch, articlePatch);
                    var extention = Path.GetExtension(files[0].FileName);
                    using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    articleVM.Article.UrlImage = @"\" + articlePatch + @"\" + fileName + extention;
                    articleVM.Article.FechaCreacion =  DateTime.Now.ToString();

                    _workContainer.Article.Add(articleVM.Article);
                    _workContainer.Save();
                    return RedirectToAction(nameof(Index));
                }

            }

            //Se agrega esta linea para que si falla no se pierda la lista de categorias en el modelo.
            articleVM.ListCategories = _workContainer.Categorie.GetCategoryList();

            return View(articleVM);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticleVM article = new ArticleVM()
            {
                Article = new BlogCore.Models.Article(),
                ListCategories = _workContainer.Categorie.GetCategoryList()

            };
            if(id != null)
            {
                article.Article = _workContainer.Article.Get(id.GetValueOrDefault());
            }
            return View(article);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticleVM articleVM)
        {
            if (ModelState.IsValid)
            {
                string mainPatch = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var articleFrom = _workContainer.Article.Get(articleVM.Article.Id);

                if (files.Count() > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string articlePatch = @"images\articles";
                    var dir = Path.GetDirectoryName(mainPatch + @"\" + articlePatch + @"\");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    var uploads = Path.Combine(mainPatch, articlePatch);
                    var extention = Path.GetExtension(files[0].FileName);
                    var newExtention = Path.GetExtension(files[0].FileName);

                    //Delete Old image
                    DeleteFileFromPatch(articleFrom.UrlImage, mainPatch);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    articleVM.Article.UrlImage = @"\" + articlePatch + @"\" + fileName + extention;
                    articleVM.Article.FechaCreacion = articleFrom.FechaCreacion;

                    _workContainer.Article.Update(articleVM.Article);
                    _workContainer.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    articleVM.Article.UrlImage = articleFrom.UrlImage;
                    articleVM.Article.FechaCreacion = articleFrom.FechaCreacion;

                }

                _workContainer.Article.Update(articleVM.Article);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));

            }

            //Se agrega esta linea para que si falla no se pierda la lista de categorias en el modelo.
            articleVM.ListCategories = _workContainer.Categorie.GetCategoryList();

            return View(articleVM);

        }

        #region
        //Call Api
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _workContainer.Article.GetAll( includeProperties: "Category");
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

            var article = _workContainer.Article.Get(id);
            string mainPatch = _hostEnvironment.WebRootPath;
            //Delete Old image
            DeleteFileFromPatch(article.UrlImage, mainPatch);
            if (article == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error borrando artículo",
                    Result = "Ok"
                });
            }
            _workContainer.Article.Remove(article);
            _workContainer.Save();

            return Json(new
            {
                success = true,
                message = "Artículo eliminada correctamente!"
            });
        }

        private static void DeleteFileFromPatch(string UrlImage, string mainPatch)
        {
            var patImagen = Path.Combine(mainPatch, UrlImage.TrimStart('\\'));
            if (System.IO.File.Exists(patImagen))
            {
                System.IO.File.Delete(patImagen);
            }
        }

        #endregion
    }
}
