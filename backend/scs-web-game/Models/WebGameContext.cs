using Microsoft.EntityFrameworkCore;
using scs_web_game.Models;
namespace scs_web_game.Models;

public partial class WebGameContext : DbContext
{
    public DbSet<Employee> Employee { get; set; }
    public DbSet<Game> Game { get; set; }
    public DbSet<Player> Player { get; set; }
    public WebGameContext()
    {
    }

    public WebGameContext(DbContextOptions<WebGameContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=BATANGA\\SQL22EXPRESS;Database=scs-web-game; Integrated Security=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);
            entity.Property(e => e.EmployeeId).HasDefaultValueSql("(newid())").HasColumnName("ID");
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ImgFileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Role).IsRequired().HasConversion<int>();
        });
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(g => g.GameId);
            entity.HasOne(g => g.Player)
                .WithMany(p => p.Game)
                .HasForeignKey(g => g.PlayerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ForeignKey_Game_Player");
            entity.Property(g => g.GameId).HasDefaultValueSql("(newid())").HasColumnName("ID");
            entity.Property(g => g.Score).IsRequired().HasDefaultValue(0);
        });
        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(p => p.PlayerId).HasDefaultValueSql("(newid())").HasColumnName("ID");
            entity.Property(p => p.PlayerEmail).HasMaxLength(100).IsRequired().UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.HasIndex(p => p.PlayerEmail).IsUnique();
            entity.Property(e => e.PlayerName).IsRequired().HasMaxLength(95);
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
