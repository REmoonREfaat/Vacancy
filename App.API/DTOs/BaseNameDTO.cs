
using System.ComponentModel.DataAnnotations;

namespace App.API.DTOs
{
    public class BaseNameDTO : BaseDTO
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Required *")]
        public string Name { get; set; }
        [Display(Name = "Name Arabic")]
        [Required(ErrorMessage = "Required *")]
        public string NameAr { get; set; }
    }
}
