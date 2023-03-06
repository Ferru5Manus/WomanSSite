using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WomanSite.Models;

namespace WomanSite.Database
{
    public class ApplicationContext:DbContext
    {
        
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(System.Environment.GetEnvironmentVariable("DB_URL"));
        }
    }
}
