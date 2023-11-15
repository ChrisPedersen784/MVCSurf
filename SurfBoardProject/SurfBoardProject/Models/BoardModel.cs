using Microsoft.AspNetCore.Mvc;
using SurfBoardProject.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfBoardProject.Models
{
    public class BoardModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Volume { get; set; }
        [NotMapped]
        public Enum.BoardType? BoardType { get; set; }
        public string? ImgUrl { get; set; }
        public string? BoardDescription { get; set; }
        public double Price { get; set; }
        public string? Equipment { get; set; }
        public int IsAvailable { get; set; } = 0;
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public ICollection<Rental>? Rentals { get; set; }



    }
}
