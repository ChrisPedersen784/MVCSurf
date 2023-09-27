using System.ComponentModel.DataAnnotations;

namespace RentalWebAPI.Models
{
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal Price { get; set; }


    }
}
