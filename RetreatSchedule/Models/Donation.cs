using RetreatSchedule.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Models
{
    public class Donation : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public decimal Amount { get; set; }

        [DisplayName("Transaction Reference")]
        public string TransactionRef { get; set; }

        [DisplayName("Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }
        
        [DisplayName("Date Created")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy hh:mm:ss}")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; }
    }
}
