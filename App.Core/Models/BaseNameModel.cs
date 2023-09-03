using System.ComponentModel.DataAnnotations;

namespace App.Core.Models
{
    public class BaseNameModel : BaseModel
    {
        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string NameAr { get; set; }
    }
}
