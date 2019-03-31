using RetreatSchedule.Models;
using RetreatSchedule.Models.Enum;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RetreatSchedule.Models
{
    public class User : BaseEntity
    {
        [ScaffoldColumn(false)]
        [DisplayName("Identity")]
        public string AspId { get; set; }

        [DisplayName("First name")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [DisplayName("Middle name")]
        public string MiddleName { get; set; }

        [DisplayName("Last name")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
                
        public string Email { get; set; }

        [DisplayName("Phone number")]
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [DisplayName("Age range")]
        [Required(ErrorMessage = "Age range is required")]
        public string AgeRange { get; set; }
        
        [DisplayName("Profile Picture")]
        public string ProfilePicture { get; set; }

        [DisplayName("Super Admin")]
        public bool? IsSuperAdmin { get; set; } = false;

        [DisplayName("Member Type")]
        public MemberType? MemberType { get; set; }

        public string Referrer { get; set; }

        [DisplayName("Referring Centre")]
        public int? ReferringCentreID { get; set; }

        public virtual Centre ReferringCentre { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
