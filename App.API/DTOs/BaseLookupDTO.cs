
﻿using System.ComponentModel.DataAnnotations;

namespace App.API.DTOs
{
    public class BaseLookupDTO : BaseNameDTO
    {
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Description Arabic")]
        public string DescriptionAr { get; set; }
    }
}