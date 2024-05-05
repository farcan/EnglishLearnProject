using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EnglishLearningProject.Models
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Word> Word { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
            
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //i did define primary key in entity.

            builder.Entity<Word>().HasKey(w => w.WordID);
            builder.Entity<Quiz>().HasKey(q => q.quizID);

            //Bir Userin birden çok kelimesi olabilir
            //Bir kelimenin bir useri olabilir.
            //


            builder.Entity<Word>()
                .HasOne(x => x.user)
                .WithMany(x => x.Words)
                .HasForeignKey(x => x.UserID)
                .IsRequired(true);

            builder.Entity<Quiz>()
                .HasOne(x => x.AppUser)
                .WithMany(x => x.quizzes)
                .HasForeignKey(x => x.UserID);

            builder.Entity<Quiz>()
                .HasOne(x=>x.Word)
                .WithMany(x=>x.Quizs)
                .HasForeignKey(x=>x.WordID)
                .IsRequired(false);

        }

    }
}
