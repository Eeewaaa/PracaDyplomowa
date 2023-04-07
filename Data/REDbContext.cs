using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RealEstate3.Models;
using System.Text.RegularExpressions;
using System.Xml;

namespace RealEstate3.Data
{
    public class REDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public REDbContext(DbContextOptions<REDbContext> options): base(options)
        {
           
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.UpperCategoryId).ValueGeneratedNever();

            });

            modelBuilder.Entity<Category>().HasData(

                new Category { Id = 1, UpperCategoryId = null, Name = "Nieruchomość", },
                new Category { Id = 2, UpperCategoryId = 1, Name = "Dom", },
                new Category { Id = 3, UpperCategoryId = 1, Name = "Mieszkanie" },
                new Category { Id = 4, UpperCategoryId = 1, Name = "Pokój" },
                new Category { Id = 5, UpperCategoryId = 2, Name = "Bliźniak" },
                new Category { Id = 6, UpperCategoryId = 2, Name = "Dom wolnostojący" },
                new Category { Id = 7, UpperCategoryId = 3, Name = "Kamienica" },
                new Category { Id = 8, UpperCategoryId = 3, Name = "Nowe budownictwo" },
                new Category { Id = 9, UpperCategoryId = 3, Name = "Inne" },
                new Category { Id = 10, UpperCategoryId = 4, Name = "Jednoosobowy" },
                new Category { Id = 11, UpperCategoryId = 4, Name = "Ze współlokatorem" }
                );
            base.OnModelCreating(modelBuilder);//default identification tabels    
        }


        public DbSet<Estate> Estates { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        //Reservation related tabels

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationItem> ReservationItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
