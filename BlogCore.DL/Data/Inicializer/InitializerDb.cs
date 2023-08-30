using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DL.Data.Inicializer
{
    public class InitializerDb :IinitializerDb
    {
        //TODO:Siembra de datos Seeding: Esto es para crear un nuevo usuario, roles y asignaccion de rol al usuario cuando 
        // hay una nueva migracion y no existen estas informaciones. 

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole>  _roleManager;

        public InitializerDb(ApplicationDbContext db, UserManager<ApplicationUser>  userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        void IinitializerDb.Initializer()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

            }
            catch (Exception)
            {

                 
            }
            if (_db.Roles.Any(x => x.Name == CNT.Admin)) return;

            //Crear Roles sino existen en una migracion
            _roleManager.CreateAsync(new IdentityRole(CNT.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.User)).GetAwaiter().GetResult();

            //Crear Usuario Inicial sino existe en una migracion
            var defUser = "admin123@gmail.com";
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = defUser,
                Email = defUser,
                EmailConfirmed = true,
                Name = "Administrator"

            }, "Prueba123**").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where(x=>x.Email == defUser).FirstOrDefault();
            _userManager.AddToRoleAsync(user, CNT.Admin).GetAwaiter().GetResult();


        }
    }
}
