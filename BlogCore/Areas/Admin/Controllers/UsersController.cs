using BlogCore.DL.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly IWebHostEnvironment _hostEnvironment;
        public UsersController(IWorkContainer workContainer, IWebHostEnvironment hostEnvironment)
        {

            _workContainer = workContainer;
            _hostEnvironment = hostEnvironment;
        }
        //[AllowAnonymous]
        public IActionResult Index()
        {
            //Opc 1 getAll users
            //return View(_workContainer.User.GetAll());
            //Opción 2: Obtener todos los usuarios menos el que esté logueado, para no bloquearse el mismo
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_workContainer.User.GetAll(u => u.Id != usuarioActual.Value));
        }
        public IActionResult Block(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            _workContainer.User.BlockUser(id);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Unlock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _workContainer.User.UnlockUser(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
