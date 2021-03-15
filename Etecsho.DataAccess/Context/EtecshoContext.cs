using Etecsho.Models.Entites.Blog;
using Etecsho.Models.Entites.ContactUs;
using Etecsho.Models.Entites.Permissions;
using Etecsho.Models.Entites.Product;
using Etecsho.Models.Entites.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Etecsho.DataAccess.Context
{
    public class EtecshoContext : DbContext
    {
        public EtecshoContext(DbContextOptions<EtecshoContext> options) : base(options)
        {

        }


        #region User

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }


        #endregion

        #region Permission

        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion

        #region Blog
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogSelectedCategory> BlogSelectedCategories { get; set; }
        public DbSet<VideoSelectedCategory> VideoSelectedCategory { get; set; }


        #endregion

        #region Comment

        public DbSet<Models.Entites.Comment.Comment> Comment { get; set; }
        public DbSet<Models.Entites.Comment.ProductType> ProductType { get; set; }


        #endregion


        #region Slider

        public DbSet<Etecsho.Models.Entites.Slider.Slider> Slider { get; set; }

        #endregion

        #region ContactUs

        public DbSet<ContactUs> ContactUs { get; set; }

        #endregion


        #region Employee

        public DbSet<Models.Entites.Employee.Employee> employee { get; set; }

        #endregion

        #region Product

        public DbSet<ProductCategories> ProductCategories { get; set; }
        public DbSet<ProductSelectedCategory> ProductSelectedCategory { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<ProductFeature> ProductFeature { get; set; }
        public DbSet<ProductGallery> ProductGallery { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;



            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Role>()
            .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<Blog>()
                .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<BlogCategory>()
                .HasQueryFilter(r => !r.IsDelete);


            modelBuilder.Entity<Video>()
                  .HasQueryFilter(r => !r.IsDelete);
            
            modelBuilder.Entity<Etecsho.Models.Entites.Comment.Comment>()
                  .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<Etecsho.Models.Entites.Slider.Slider>()
                  .HasQueryFilter(r => !r.IsDelete);

             modelBuilder.Entity<Etecsho.Models.Entites.Employee.Employee>()
                  .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<ProductCategories>()
                .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<Product>()
                .HasQueryFilter(r => !r.IsDelete);


            base.OnModelCreating(modelBuilder);
        }


    }
}
