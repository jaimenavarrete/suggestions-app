using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Data.Database
{
    public partial class SuggestionsAppContext : DbContext
    {
        public SuggestionsAppContext()
        {
        }

        public SuggestionsAppContext(DbContextOptions<SuggestionsAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<Suggestion> Suggestions { get; set; } = null!;
        public virtual DbSet<Upvote> Upvotes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.ColorHexCode)
                    .HasMaxLength(7);
            });

            modelBuilder.Entity<Suggestion>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasColumnType("text");

                entity.Property(e => e.UpvotesAmount);

                entity.Property(e => e.CategoryId);

                entity.Property(e => e.StateId);

                entity.Property(e => e.UserId);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime");

                entity.Property(e => e.Approved);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Suggestions)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Categories_Suggestions");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Suggestions)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK_States_Suggestions");
            });

            modelBuilder.Entity<Upvote>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.SuggestionId);

                entity.Property(e => e.UserId);

                entity.HasOne(d => d.Suggestion)
                    .WithMany(p => p.Upvotes)
                    .HasForeignKey(d => d.SuggestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Suggestions_Upvotes");
            });
        }
    }
}
