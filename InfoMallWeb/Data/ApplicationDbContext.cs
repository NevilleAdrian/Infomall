using System;
using System.Collections.Generic;
using System.Text;
using InfoMallWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfoMallWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BannerInformation> BannersInformation { get; set; }
        public DbSet<CategoryForInformation> CategoriesForInformation { get; set; }
        public DbSet<CategoryForTab> CategoriesForTab { get; set; }
        public DbSet<Clientele> Clienteles { get; set; }
        public DbSet<ContactInformation> ContactsInformation { get; set; }
        public DbSet<ContentForMall> ContentsForMall { get; set; }
        public DbSet<ContentForTab> ContentsForTab { get; set; }
        public DbSet<ContentImage> ContentImages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<CustomerProduct> CustomerProducts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionCustomer> PromotionCustomers { get; set; }
        public DbSet<PromotionInformation> PromotionsInformation { get; set; }
        public DbSet<Author> Author { get; set; }
    }
}
