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
        /*
         * 
         *  cs_service:
    build: WomanSSite
    volumes:
      - ./config.json:/app/config.json
    ports:
      - 80:80
    depends_on:
      - db
    deploy:
      resources:
        limits:
          memory: 1874M
    environment:
      - "DB_URL=Server=db; Database=postgres; Uid=postgres;Pwd=78aaJJxHg;"
    restart: always
         */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=db; Database=postgres; Uid=postgres;Pwd=78aaJJxHg;");
        }
    }
}
