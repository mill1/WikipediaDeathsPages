using Microsoft.EntityFrameworkCore;

namespace WikipediaDeathsPages.Data.Models
{
    public class WRContext : DbContext
    {
        public virtual DbSet<Olympians> Olympians { get; set; }
        public virtual DbSet<References> References { get; set; }
        public virtual DbSet<Sources> Sources { get; set; }

        public WRContext()
        {
        }

        public WRContext(DbContextOptions<WRContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Olympians>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FullGivenName).HasMaxLength(255);

                entity.Property(e => e.FullSurname).HasMaxLength(255);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Noc1)
                    .HasColumnName("NOC1")
                    .HasMaxLength(3);

                entity.Property(e => e.Noc2)
                    .HasColumnName("NOC2")
                    .HasMaxLength(3);

                entity.Property(e => e.Noc3)
                    .HasColumnName("NOC3")
                    .HasMaxLength(3);

                entity.Property(e => e.Noc4)
                    .HasColumnName("NOC4")
                    .HasMaxLength(3);

                entity.Property(e => e.Season).HasMaxLength(1);

                entity.Property(e => e.Sport1).HasMaxLength(3);

                entity.Property(e => e.Sport2).HasMaxLength(3);

                entity.Property(e => e.Sport3).HasMaxLength(3);

                entity.Property(e => e.Sport4).HasMaxLength(3);

                entity.Property(e => e.UsedGivenName).HasMaxLength(255);

                entity.Property(e => e.UsedSurname).HasMaxLength(255);
            });

            modelBuilder.Entity<References>(entity =>
            {
                entity.HasIndex(e => e.SourceCode);

                entity.Property(e => e.AccessDate).HasColumnType("date");
                entity.Property(e => e.ArchiveDate).HasColumnType("date");

                entity.Property(e => e.ArticleTitle)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Date).HasColumnType("date");
                entity.Property(e => e.DeathDate).HasColumnType("date");

                entity.Property(e => e.SourceCode)
                    .IsRequired()
                    .HasMaxLength(35);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(35);

                entity.HasOne(d => d.SourceCodeNavigation)
                    .WithMany(p => p.References)
                    .HasForeignKey(d => d.SourceCode);
            });

            modelBuilder.Entity<Sources>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code).HasMaxLength(35);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

        }

    }
}
