using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace AIFormrecognizer.Models
{
    public class AiformDbContext : DbContext
    {
       
        public DbSet<Invoice> Invoices { get; set; }


        public AiformDbContext(DbContextOptions<AiformDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
