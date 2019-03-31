using RetreatSchedule.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace RetreatSchedule.ViewModel
{
    public class PaymentRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(11)]
        [Display(Name = "Phone number")]
        public string PhoneNo { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Number of spaces")]
        public int NoOfSpaces { get; set; }

        [Required]
        [Display(Name = "Age range")]
        public string AgeRange { get; set; }

        public int ActivityID { get; set; }

        public PaymentType PaymentType { get; set; }

        public string Referrer { get; set; }

        public int? ReferringCentreID { get; set; }
    }
}
