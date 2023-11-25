using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Product.Models
{
    public class Surfboard
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Volume { get; set; }

        public string? ImgUrl { get; set; }
        public string? BoardDescription { get; set; }
        public double Price { get; set; }
        public string? Equipment { get; set; }
        public int IsAvailable { get; set; } = 0;
        [Timestamp]
        public byte[] RowVersion { get; set; }


        public string FullUrl 
        {
            get
            {
                return string.Format("/product/" + Id.ToString().ToLower());
            }
            //set
            //{
            //    Id = int.Parse(value);
            //}
        }

        public Surfboard()
        {
            
        }



        public Surfboard(int id, string name, double length, double width, double volume, double v, string? imgUrl, string boardDescription, double price, string equipment, int isAvailable, byte[] rowVersion)
        {
            Id = id;
            Name = name;
            Length = length;
            Width = width;
            Volume = volume;
            ImgUrl = imgUrl;
            BoardDescription = boardDescription;
            Price = price;
            Equipment = equipment;
            IsAvailable = isAvailable;
            RowVersion = rowVersion;
        }
    }
}

