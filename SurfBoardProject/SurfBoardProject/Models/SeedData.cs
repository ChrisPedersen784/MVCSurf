using Microsoft.EntityFrameworkCore;
using SurfBoardProject.Data;

namespace SurfBoardProject.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SurfBoardProjectContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<SurfBoardProjectContext>>()))
            {
                // Look for any movies.
                if (context.BoardModel.Any())
                {
                    return;   // DB has been seeded
                }

                context.BoardModel.AddRange(
                    new BoardModel
                    {
                        Name = "The Minilog",
                        Length = 6,
                        Width = 21,
           
                        Volume = 38.8,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                        
                    },

                    new BoardModel
                    {
                        Name = "The Wide Glider",
                        Length = 7.1,
                        Width = 21.75,
              
                        Volume = 44.16,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                    },

                    new BoardModel
                    {
                        Name = "The Golden Ratio",
                        Length = 6.3,
                        Width = 21.85,
               
                        Volume = 43.22,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                    },

                    new BoardModel
                    {
                        Name = "Mahi Mahi",
                        Length = 5.4,
                        Width = 20.75,
                       
                        Volume = 29.39,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                    },


                    new BoardModel
                    {
                        Name = "The Emerald Glider",
                        Length = 9.2,
                        Width = 22.8,
                   
                        Volume = 65.4,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                    },

                    new BoardModel
                    {
                        Name = "The Bomb",
                        Length = 5.5,
                        Width = 21,
                   
                        Volume = 33.7,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                    },


                    new BoardModel
                    {
                        Name = "Walden Magic",
                        Length = 9.6,
                        Width = 19.4,
                     
                        Volume = 80,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                    },

                    new BoardModel
                    {
                        Name = "Naish One",
                        Length = 12.6,
                        Width = 30,
                    
                        Volume = 301,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                    },

                    new BoardModel
                    {
                        Name = "Six Tourer",
                        Length = 11.6,
                        Width = 32,
               
                        Volume = 270,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                    },

                    new BoardModel
                    {
                        Name = "Naish Maliko",
                        Length = 14,
                        Width = 25,
                    
                        Volume = 330,
                        BoardDescription = "Fish",
                        Price = 565,
                        Equipment = "Paddle",
                    }


                );
                context.SaveChanges();
            }
        }
    }
}