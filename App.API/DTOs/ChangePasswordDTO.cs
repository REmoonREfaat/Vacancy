﻿using System.ComponentModel.DataAnnotations;

namespace App.API.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
