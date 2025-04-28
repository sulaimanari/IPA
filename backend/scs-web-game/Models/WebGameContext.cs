using Microsoft.EntityFrameworkCore;
using scs_web_game.Models;
namespace scs_web_game.Models;

public partial class WebGameContext : DbContext
{
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
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
