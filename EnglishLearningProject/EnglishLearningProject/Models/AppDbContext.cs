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
        public DbSet<TestLog> TestLog { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //i did define primary key in entity.

            builder.Entity<Word>().HasKey(w => w.WordID);
            builder.Entity<Quiz>().HasKey(q => q.quizID);
            builder.Entity<TestLog>().HasKey(x => x.TestLogID);

            //Bir Userin birden çok kelimesi olabilir
            //Bir kelimenin bir useri olabilir.
            //


            builder.Entity<Word>()
                .HasOne(x => x.user)
                .WithMany(x => x.Words)
                .HasForeignKey(x => x.UserID)
                .IsRequired(false);



            builder.Entity<Quiz>()
                .HasOne(x => x.AppUser)
                .WithMany(x => x.quizzes)
                .HasForeignKey(x => x.UserID)
                .IsRequired(false);



            builder.Entity<Quiz>()
                .HasOne(x => x.Word)
                .WithMany(x => x.Quizs)
                .HasForeignKey(x => x.WordID)
                .IsRequired(false);



            builder.Entity<TestLog>().
                HasOne(x => x.Quiz)
                .WithMany(x => x.testLogs)
                .HasForeignKey(x => x.QuizID)
                .IsRequired(false);
            

         


        }

    }
}
