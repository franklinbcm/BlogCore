using BlogCore.Data;
using BlogCore.DL.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly IWebHostEnvironment _hostEnvironment;
        public SlidersController(IWorkContainer workContainer, IWebHostEnvironment hostEnvironment)
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
            SliderVM slider = new SliderVM()
            {
                Slider = new BlogCore.Models.Slider(),
                ListSliderStatus = new BlogCore.Models.ViewModels.SliderVM().ListSliderStatus


            };
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SliderVM sliderVM)
        {
            if (ModelState.IsValid)
            {
                string mainPatch = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(sliderVM.Slider.Id == 0)
                {
                    if (files.Count() > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        string articlePatch = @"images\sliders";
                        var dir = Path.GetDirectoryName(mainPatch + @"\" + articlePatch + @"\");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        var uploads = Path.Combine(mainPatch, articlePatch);
                        var extention = Path.GetExtension(files[0].FileName);
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        sliderVM.Slider.UrlImage = @"\" + articlePatch + @"\" + fileName + extention;
                    }
                    else
                    {
                        sliderVM.Slider.UrlImage = String.Empty;
                    }

                    _workContainer.Slider.Add(sliderVM.Slider);
                    _workContainer.Save();
                    return RedirectToAction(nameof(Index));
                }

            }


            return View(sliderVM);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            SliderVM slider = new SliderVM()
            {
                Slider = new BlogCore.Models.Slider(),
                ListSliderStatus = new BlogCore.Models.ViewModels.SliderVM().ListSliderStatus


            };
            if (id != null)
            {
                slider.Slider = _workContainer.Slider.Get(id.GetValueOrDefault());
            }
            return View(slider);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SliderVM sliderVM)
        {
            if (ModelState.IsValid)
            {
                string mainPatch = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var sliderFrom = _workContainer.Slider.Get(sliderVM.Slider.Id);

                if (files.Count() > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string articlePatch = @"images\Sliders";
                    var dir = Path.GetDirectoryName(mainPatch + @"\" + articlePatch + @"\");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    var uploads = Path.Combine(mainPatch, articlePatch);
                    var extention = Path.GetExtension(files[0].FileName);
                    var newExtention = Path.GetExtension(files[0].FileName);

                    //Delete Old image
                    DeleteFileFromPatch(sliderFrom.UrlImage, mainPatch);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    sliderVM.Slider.UrlImage = @"\" + articlePatch + @"\" + fileName + extention;

                    _workContainer.Slider.Update(sliderVM.Slider);
                    _workContainer.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    sliderVM.Slider.UrlImage = sliderFrom.UrlImage;

                }

                _workContainer.Slider.Update(sliderVM.Slider);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));

            }


            return View(sliderVM);

        }

        #region
        //Call Api
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _workContainer.Slider.GetAll();
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

            var slider = _workContainer.Slider.Get(id);
            string mainPatch = _hostEnvironment.WebRootPath;
            //Delete Old image
            DeleteFileFromPatch(slider.UrlImage, mainPatch);
            if (slider == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error borrando artículo",
                    Result = "Ok"
                });
            }
            _workContainer.Slider.Remove(slider);
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
