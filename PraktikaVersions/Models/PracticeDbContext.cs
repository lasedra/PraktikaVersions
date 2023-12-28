using Microsoft.EntityFrameworkCore;

namespace PraktikaVersions.Models;

public partial class PracticeDbContext : DbContext
{
    public static PracticeDbContext dbContext = new PracticeDbContext();

    public PracticeDbContext()
    {
    }

    public PracticeDbContext(DbContextOptions<PracticeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=localhost;Database=PracticeDB;UserName=postgres;Password=password");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.id).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.patronymic)
                .HasMaxLength(50)
                .HasColumnName("patronymic");
            entity.Property(e => e.surname)
                .HasMaxLength(50)
                .HasColumnName("surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
