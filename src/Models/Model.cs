using System.Collections.Generic;
using System.Linq;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=reading_list_api");
      }
    }
  }
}