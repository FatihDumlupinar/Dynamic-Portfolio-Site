using DynamicPortfolioSite.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DynamicPortfolioSite.Repository.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContext) : base(dbContext)
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Localization> Localization { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectAndCategory> ProjectAndCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<Work> Works { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Education> Educations { get; set; }

    }
}
