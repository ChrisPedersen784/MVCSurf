using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurfBoardProject.Models;

namespace SurfBoardProject.Data
{
    public class SurfBoardProjectContext : IdentityDbContext
    {
        public SurfBoardProjectContext (DbContextOptions<SurfBoardProjectContext> options)
            : base(options)
        {
        }

        public DbSet<SurfBoardProject.Models.BoardModel> BoardModel { get; set; } = default!;
        public DbSet<SurfBoardProject.Models.Rental> Rental { get; set; } = default!;
        public DbSet<SurfBoardProject.Models.Customer> Customer { get; set; } = default!;

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<BoardModel>()
        //        .HasMany(x => x.Rentals)
        //        .WithMany(y => y.Boards)
        //        .UsingEntity(j => j.ToTable("BoardRental"));

        //    builder.Entity<Customer>()
        //        .HasMany(x => x.Rentals)
        //        .WithMany(y => y.Customers)
        //        .UsingEntity(j => j.ToTable("CustomerRental"));
        //}

    }
}
