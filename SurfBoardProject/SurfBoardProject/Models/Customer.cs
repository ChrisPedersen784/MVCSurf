using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfBoardProject.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [NotMapped]
        public int CustomerNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Rental>? Rentals { get; set; }
        public int UserId { get; set; }
        public IdentityUser? IdentityUser { get; set; }

        
    }
}
