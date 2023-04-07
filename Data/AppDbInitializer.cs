using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealEstate3.Data.Enums;
using RealEstate3.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using RealEstate3.Data.Static;
using Azure.Identity;

namespace RealEstate3.Data
{
    public class AppDbInitializer
    {   
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if(!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                if (!await roleManager.RoleExistsAsync(UserRoles.Owner))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Owner));
                if (!await roleManager.RoleExistsAsync(UserRoles.TemporaryOwner))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.TemporaryOwner));
                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var adminUser = await userManager.FindByEmailAsync("admin@realEstates.com");
                if(adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        //FullName = "Admin User",
                        UserName = "admin-user",
                        Email = "admin@realEstates.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "ProjectAdmin@123?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@realEstates.com";
                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        //FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "ProjectUser@123?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }

                string appOwnerEmail1 = "MKowalski@realEstates.com";
                var appOwner1 = await userManager.FindByEmailAsync(appOwnerEmail1);
                if (appOwner1 == null)
                {
                    var newAppOwner1 = new ApplicationUser()
                    {
                        UserName = "MKowalski",
                        Email = appOwnerEmail1,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppOwner1, "Kowalski#123?");
                    await userManager.AddToRoleAsync(newAppOwner1, UserRoles.Owner);
                }

                string appOwnerEmail2 = "EMalinowska@realEstates.com";
                var appOwner2 = await userManager.FindByEmailAsync(appOwnerEmail2);
                if (appOwner2 == null)
                {
                    var newAppOwner2 = new ApplicationUser()
                    {
                        UserName = "EMalinowska",
                        Email = appOwnerEmail2,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppOwner2, "Malinowska#123?");
                    await userManager.AddToRoleAsync(newAppOwner2, UserRoles.Owner);
                }

                string appOwnerEmail3 = "MSochacki@realEstates.com";
                var appOwner3 = await userManager.FindByEmailAsync(appOwnerEmail3);
                if (appOwner3 == null)
                {
                    var newAppOwner3 = new ApplicationUser()
                    {
                        UserName = "MSochacki",
                        Email = appOwnerEmail3,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppOwner3, "Sochacki#123?");
                    await userManager.AddToRoleAsync(newAppOwner3, UserRoles.Owner);
                }

                string appOwnerEmail4 = "LZajac@realEstates.com";
                var appOwner4 = await userManager.FindByEmailAsync(appOwnerEmail4);
                if (appOwner4 == null)
                {
                    var newAppOwner4 = new ApplicationUser()
                    {
                        //FullName = "Application Owner",
                        UserName = "LZajac",
                        Email = appOwnerEmail4,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppOwner4, "Zajac#123?");
                    await userManager.AddToRoleAsync(newAppOwner4, UserRoles.Owner);
                }
            }
        }
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context =  serviceScope.ServiceProvider.GetService<REDbContext>();

               context.Database.EnsureCreated();

                //Owners
                if (!context.Owners.Any())
                {
                    context.Owners.AddRange(new List<Owner>()
                    {
                        new Owner() {
                            Name = "Marcin",
                            Surname = "Kowalski",
                            Birthday = new DateTime(1986, 11, 23),
                            ContactNumber = "794507321",
                            Gender = Enums.Gender.male,
                            ProfilePictureUrl = "https://localhost:7150/images/ownersGallery/MarcinKowalski.jpg", 
                            UserId = "332b9601-89e7-4605-b5c0-276f14026642"
                        },
                         new Owner() {
                            Name = "Ewa",
                            Surname = "Malinowska",
                            Birthday = new DateTime(1976, 3, 15),
                            ContactNumber = "791526871",
                            Gender = Enums.Gender.female,
                            ProfilePictureUrl= "https://localhost:7150/images/ownersGallery/EwaMalinowska.jpg",
                            UserId = "9b6e5b8f-84a9-4713-a4de-c08f9f21ca9b"
                         },
                         new Owner() {
                            Name = "Miłosz",
                            Surname = "Sochacki",
                            Birthday = new DateTime(1994, 5, 21),
                            ContactNumber = "500115326",
                            Gender = Enums.Gender.male,
                            ProfilePictureUrl= "https://localhost:7150/images/ownersGallery/MiloszSochacki.jpg",
                            UserId = "edc69f41-5f5b-40ab-8b3f-ff3ed0a809d9"
                         },
                         new Owner() {
                            Name = "Liliana",
                            Surname = "Zając",
                            Birthday = new DateTime(1975, 2, 16),
                            ContactNumber = "502125325",
                            Gender = Enums.Gender.female,
                            ProfilePictureUrl= "https://localhost:7150/images/ownersGallery/LilianaZajac.jpg",
                            UserId = "818e6d95-fc33-4bf7-801c-b54f79d4888f"
                         },
                    });
                    context.SaveChanges();
                }


                    //Addresses
                    if (!context.Addresses.Any())
                {
                    context.Addresses.AddRange(new List<Address>()
                    {
                        new Address()
                        {
                            Street = "Akacjowa",
                            Number = "23/1",
                            PostalCode = "53-322",
                        },

                           new Address()
                        {
                            Street = "Mydlana",
                            Number = "15",
                            PostalCode = "50-542",
                        },

                               new Address()
                        {
                            Street = "Zwycięska",
                            Number = "5/2",
                            PostalCode = "51-562",
                        },

                              new Address()
                        {
                            Street = "Kwidzyńska",
                            Number = "16b/7",
                            PostalCode = "51-432",
                        },

                           new Address()
                        {
                            Street = "Zakładowa",
                            Number = "1/2",
                            PostalCode = "50-502",
                        },

                               new Address()
                        {
                            Street = "Piękna",
                            Number = "50A/1",
                            PostalCode = "50-506",
                        }
                    });

                    context.SaveChanges();
                }


                if (!context.Estates.Any())
                {
                    context.Estates.AddRange(new List<Estate>()
                    {
                        new Estate() {
                            Title = "Słoneczne",
                            AvaiableFrom = new DateTime(2023,1,1),
                            Balcony = true,
                            Garden = false,
                            PetFriendly = false,
                            Rent= 3500,
                            Size= 45.0,
                            Description = "Piękne słoneczne mieszkanie, dwustronne. 3 pokoje i osobno kuchnia. Łazienka wyposażona w wannę, kuchnia w piekarnik, zmywarkę oraz mikrofalę",
                            CategoryId = 9,
                            OwnerId = 7,
                            AddressId = 1,
                        },

                        new Estate() {
                            Title = "Bliżniak",
                            AvaiableFrom = new DateTime(2023,2,1),
                            Balcony = false,
                            Garden = true,
                            PetFriendly = true,
                            Rent= 5500,
                            Size= 100,
                            Description = "Bliżniak dwa piętra oraz poddasze. Na poddaszu urządzona sypialnia główna, na piętrze dwa pokoje dla dzieci oraz łazienka.",
                            OwnerId = 8,
                            CategoryId= 5,
                            AddressId = 2,
                        },

                        new Estate() {
                            Title = "Mieszkanie - kawalerka",
                            AvaiableFrom = new DateTime(2023,2,13),
                            Balcony = false,
                            Garden = true,
                            PetFriendly = true,
                            Rent= 2900,
                            Size= 30,
                            Description = "Kawalerka na parterze. Łazienka z prysznicem. W pobliżu znajduje się piekarnia, Żabka oraz przedszkole",
                            OwnerId = 8,
                            CategoryId = 8,
                            AddressId = 3,
                        },
                                new Estate() {
                            Title = "Pokój dla studenta",
                            AvaiableFrom = new DateTime(2023,3,1),
                            Balcony = false,
                            Garden = false,
                            PetFriendly = true,
                            Rent= 1200,
                            Size= 9,
                            Description = "Pokój jednoosobowy wyposażony w łóżko, szafę i biurku. W mieszkaniu znajduje się drugi pokój o podobnej wielkości zajmowany przez studenta medycyny. Wspólny salon z aneksem kuchennym i łazienka.",
                            OwnerId = 9,
                            CategoryId = 10,
                            AddressId = 4,
                        },
                                new Estate() {
                            Title = "Pokój dla pary",
                            AvaiableFrom = new DateTime(2023,2,1),
                            Balcony = false,
                            Garden = false,
                            PetFriendly = false,
                            Rent= 1600,
                            Size= 14,
                            Description = "Pokój dwuosobowy z pojedyńczym łóżkiem, najlepiej dla pary. W mieszkaniu wspólna kuchnia i łazienka. Pokój wyposażony w dużą szafę, biurko, sofę i TV",
                            OwnerId = 10,
                            CategoryId = 11,
                            AddressId = 5,
                        },
                                new Estate() {
                            Title = "Mieszkanie blisko centrum",
                            AvaiableFrom = new DateTime(2023,1,10),
                            Balcony = true,
                            Garden = false,
                            PetFriendly = true,
                            Rent= 3800,
                            Size= 65,
                            Description = "Mieszkanie na Tarnogaju, w pobliżu place zabaw, przychodnia oraz przedszkola i szkoła publiczna. Mieszkanie 3 pokojowe z sypialnią, pokojem dziecięcym i salonem. Wydzielona kuchnia, łazienka i mały schowek/garderoba.",
                            OwnerId = 10,
                            CategoryId = 8,
                            AddressId = 8,
                        }
                    });
                    context.SaveChanges();

                }

                //Pictures
                if (!context.Pictures.Any())
                {
                    //string path = "C:\\Users\\Ewa\\source\\repos\\RealEstate\\RealEstate3\\wwwroot\\images\\estatesGallery\\";
                    context.Pictures.AddRange(new List<Picture>()
                    {
                        //Nieruchomosc 1
                        new Picture(){
                         
                            OriginalName = "1",
                            PictureURL = "https://localhost:7150/images/estatesGallery/1.jpg",
                            EstateId = 27
                        },

                        new Picture(){

                            OriginalName = "1b",
                            PictureURL = "https://localhost:7150/images/estatesGallery/1b.jpg",
                            EstateId = 27
                        },

                        new Picture(){

                            OriginalName = "1c",
                            PictureURL = "https://localhost:7150/images/estatesGallery/1c.jpg",
                            EstateId = 27
                        },

                         new Picture(){

                            OriginalName = "1d",
                            PictureURL = "https://localhost:7150/images/estatesGallery/1d.jpg",
                            EstateId = 27
                        },

                        //Nieruchomosc 2

                        new Picture(){

                            OriginalName = "2a",
                            PictureURL = "https://localhost:7150/images/estatesGallery/2a.jpg",
                            EstateId = 28
                        },

                        new Picture(){

                            OriginalName = "2b",
                            PictureURL = "https://localhost:7150/images/estatesGallery/2b.jpg",
                            EstateId = 28
                        },

                         new Picture(){

                            OriginalName = "2c",
                            PictureURL = "https://localhost:7150/images/estatesGallery/2c.jpg",
                            EstateId = 28
                        },

                         //Nieruchomosc 3
                          new Picture(){

                            OriginalName = "3",
                            PictureURL = "https://localhost:7150/images/estatesGallery/3.jpg",
                            EstateId = 29
                        },

                           new Picture(){

                            OriginalName = "3a",
                            PictureURL = "https://localhost:7150/images/estatesGallery/3a.jpg",
                            EstateId = 29
                        },

                            new Picture(){

                            OriginalName = "3c",
                            PictureURL = "https://localhost:7150/images/estatesGallery/3c.jpg",
                            EstateId = 29
                        },

                            //Nieruchomosc 4
                              new Picture(){

                            OriginalName = "4",
                            PictureURL = "https://localhost:7150/images/estatesGallery/4.jpg",
                            EstateId = 30
                        },
                                new Picture(){

                            OriginalName = "4a",
                            PictureURL = "https://localhost:7150/images/estatesGallery/4a.jpg",
                            EstateId = 30
                        },

                                //Nieruchomość 5
                                new Picture(){

                            OriginalName = "5",
                            PictureURL = "https://localhost:7150/images/estatesGallery/5.jpg",
                            EstateId = 31
                        },
                                new Picture(){

                            OriginalName = "5a",
                            PictureURL = "https://localhost:7150/images/estatesGallery/5a.jpg",
                            EstateId = 31
                        },
                                //Nieruchomosc 6
                                     new Picture(){

                            OriginalName = "6",
                            PictureURL = "https://localhost:7150/images/estatesGallery/6.jpg",
                            EstateId = 32
                        },
                                new Picture(){

                            OriginalName = "6a",
                            PictureURL = "https://localhost:7150/images/estatesGallery/6a.jpg",
                            EstateId = 32
                        },
                                    new Picture(){

                            OriginalName = "6b",
                            PictureURL = "https://localhost:7150/images/estatesGallery/6b.jpg",
                            EstateId = 32
                        },
                    });
                    context.SaveChanges();

                }

            }
        }

     
    }

}
