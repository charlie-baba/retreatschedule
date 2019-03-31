using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Poco
{
    public class DonationRequest
    {
        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(11)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
