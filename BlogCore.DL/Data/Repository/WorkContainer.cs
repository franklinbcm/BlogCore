using BlogCore.Data;
using BlogCore.DL.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DL.Data.Repository
{
    public class WorkContainer : IWorkContainer
    {
        private readonly ApplicationDbContext _db;
        public WorkContainer(ApplicationDbContext db)
        {
            _db = db;
            Categorie = new CategoryRepository(_db);
            Article = new ArticleRepository(_db);
            Slider = new SliderRepository(_db);
        }


        public ICategoryRepository Categorie { get; private set; }

        public IArticleRepository Article { get; private set; }
        public ISliderRepository Slider { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
