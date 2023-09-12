using System.ComponentModel.DataAnnotations.Schema;

namespace SurfBoardProject.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [NotMapped]
        public int CustomerNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int? RentalId { get; set; }
        public Rental? Rental { get; set; }
    }
}
