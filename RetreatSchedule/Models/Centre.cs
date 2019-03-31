using System.ComponentModel.DataAnnotations;

namespace RetreatSchedule.Models
{
    public class Centre : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Address { get; set; }
    }
}
