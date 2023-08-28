using BlogCore.DL.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogCore.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogCore.DL.Data.Repository
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _db;
        public ArticleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Article article)
        {
            var aricleItem = _db.Articles.FirstOrDefault(item => item.Id == article.Id);
            aricleItem.Name = article.Name;
            aricleItem.Description = article.Description;
            aricleItem.UrlImage = article.UrlImage;
            aricleItem.CategoryId = article.CategoryId;

            _db.SaveChanges();
        }
    }
}
