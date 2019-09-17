using Microsoft.EntityFrameworkCore;

namespace reading_list_api.Models
{
  public class ReadingListApiContext : DbContext
  {
    public ReadingListApiContext()
    { }

    public ReadingListApiContext(DbContextOptions<ReadingListApiContext> options)
      : base(options)
    { }
    public DbSet<User> Users { get; set; }
    public DbSet<ReadingList> ReadingLists { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Book>()
      .Property(e => e.Authors)
      .HasConversion(
        v => string.Join(',', v),
        v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=reading_list_api");
      }
    }
  }
}