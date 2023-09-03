using System;

namespace App.API.DTOs
{
    public class VacancyDTO : BaseLookupDTO
    {
        public int MaxApplicant { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
