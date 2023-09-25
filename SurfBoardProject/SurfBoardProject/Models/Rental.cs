using System.ComponentModel.DataAnnotations;

namespace SurfBoardProject.Models
{
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal Price { get; set; }

        public ICollection<BoardModel>? Boards { get; set; }
        public ICollection<Customer>? Customers { get; set; }


    }
}
