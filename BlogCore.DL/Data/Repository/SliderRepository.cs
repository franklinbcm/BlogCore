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
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;
        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Slider slider)
        {
            var sliderItem = _db.Sliders.FirstOrDefault(item => item.Id == slider.Id);
            sliderItem.Name = slider.Name;
            sliderItem.UrlImage = slider.UrlImage;
            sliderItem.State = slider.State;

            _db.SaveChanges();
        }
    }
}
