using BlogCore.DL.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogCore.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogCore.DL.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void BlockUser(string IdUser)
        {
            var currenUser = _db.Users.FirstOrDefault(u => u.Id == IdUser);
            currenUser.LockoutEnd = DateTime.Now.AddDays(30);
            _db.SaveChanges();
        }


        public void UnlockUser(string IdUser)
        {
            var currenUser = _db.Users.FirstOrDefault(u => u.Id == IdUser);
            currenUser.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
