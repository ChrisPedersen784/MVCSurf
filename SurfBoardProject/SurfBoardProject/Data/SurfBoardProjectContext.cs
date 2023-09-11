using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SurfBoardProject.Models;

namespace SurfBoardProject.Data
{
    public class SurfBoardProjectContext : DbContext
    {
        public SurfBoardProjectContext (DbContextOptions<SurfBoardProjectContext> options)
            : base(options)
        {
        }

        public DbSet<SurfBoardProject.Models.BoardModel> BoardModel { get; set; } = default!;
    }
}
