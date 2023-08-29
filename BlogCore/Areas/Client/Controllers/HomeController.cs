using BlogCore.DL.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogCore.Areas.Client.Controllers
{
    [Area("Client")]
    public class HomeController : Controller
    {

        private readonly IWorkContainer _workContainer;
        public HomeController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Slider = _workContainer.Slider.GetAll(),
                ListArticles = _workContainer.Article.GetAll()
            };
            //Esta linea es para saber si se esta ejecutando el slider desde el home o no
            ViewBag.IsHome = true;

            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            var dataDetails = _workContainer.Article.Get(id);
            return View(dataDetails);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}