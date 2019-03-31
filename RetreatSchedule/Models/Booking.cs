using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RetreatSchedule.Models.Enum;

namespace RetreatSchedule.Models
{
    public class Booking : BaseEntity
    {
        [DisplayName("Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }

        public decimal Amount { get; set; }

        [DisplayName("Payment Type")]
        public PaymentType PaymentType { get; set; }

        [DisplayName("Transaction Reference")]
        public string TransactionRef { get; set; }

        [DisplayName("Number of spaces")]
        public int NoOfSpaces { get; set; }

        [DisplayName("Date Created")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy hh:mm:ss}")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; }

        [DisplayName("Attended")]
        public bool? IsAttended { get; set; }

        [DisplayName("Activity")]
        public int ActivityID { get; set; }

        [DisplayName("User")]
        public int UserID { get; set; }

        public virtual Activity Activity { get; set; }

        public virtual User User { get; set; }
    }
}
