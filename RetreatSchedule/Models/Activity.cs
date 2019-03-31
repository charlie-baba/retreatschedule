using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RetreatSchedule.Models
{
    public class Activity : BaseEntity
    {
        [Required(ErrorMessage = "Title is requried")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Picture Link is required")]
        [DisplayName("Picture Link")]
        public string PictureUrl { get; set; }

        public int Capacity { get; set; }

        [Required(ErrorMessage = "Start Date is requried")]
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is requried")]
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Amount is requried")]
        public decimal Amount { get; set; }

        [DisplayName("House Keeping Info")]
        public string HouseKeepingInfo { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; } = true;
        
        [Required(ErrorMessage = "Location is requried")]
        [DisplayName("Location")]
        public int LocationID { get; set; }

        [Required(ErrorMessage = "Activity Type is requried")]
        [DisplayName("Activity Type")]
        public int ActivityTypeID { get; set; }

        public virtual Location Location { get; set; }

        [DisplayName("Activity Type")]
        public virtual ActivityType ActivityType { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
