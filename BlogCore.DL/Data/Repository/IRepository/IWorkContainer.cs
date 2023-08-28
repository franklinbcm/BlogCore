using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DL.Data.Repository.IRepository
{
    public interface IWorkContainer : IDisposable
    {
        //Add All repositories
        ICategoryRepository Categorie{ get; }
        IArticleRepository Article { get; }
        ISliderRepository Slider { get; }
        
        void Save();
    }
}
