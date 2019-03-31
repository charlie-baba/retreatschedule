using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RetreatSchedule.Models
{
    public class Location: BaseEntity
    {
        [Required(ErrorMessage = "Name is requried")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [DisplayName("Picture Link")]
        public string PictureUrl { get; set; }

        [DisplayName("Contact Phone")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Map Link is requried")]
        [DisplayName("Map Link")]
        public string MapUrl { get; set; }

        public string Coordinate { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}
