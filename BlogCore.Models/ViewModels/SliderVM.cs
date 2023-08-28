using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
    public class SliderVM
    {
        public Slider Slider { get; set; }
        public IEnumerable<SelectListItem> ListSliderStatus  = new List<SelectListItem>()
        {
            new SelectListItem(){ Text = "Activo", Value="true"},
            new SelectListItem(){ Text = "Inactivo", Value="false"},
        };
    }
}
