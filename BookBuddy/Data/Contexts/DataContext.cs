using BookBuddy.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.Data.Contexts
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<UserProfileEntity> Profiles { get; set; }
        public DbSet<QuizResultEntity> QuizResults { get; set; }
        public DbSet<ChapterResultEntity> ChapterResults { get; set; }
        public DbSet<QuestionResultEntity> QuestionResults { get; set; }

    }
}