using Microsoft.AspNetCore.Mvc.Rendering;

namespace rajiunschool.Models
{
    public class Dropdownmodel
    {
        public string SelectedOption { get; set; } // Stores selected value
        public List<SelectListItem> Options { get; set; } 
    }
}
