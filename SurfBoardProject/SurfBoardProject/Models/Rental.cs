namespace SurfBoardProject.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }

        public ICollection<BoardModel> Boards { get; set; }

    }
}
