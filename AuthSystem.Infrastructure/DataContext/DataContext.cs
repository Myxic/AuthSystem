using Microsoft.EntityFrameworkCore;
using AuthSystem.core.Entities;
using Microsoft.Extensions.Configuration;




namespace AuthSystem.Core.Infrastructure.DataContext;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> option) : base(option)
    { }

    public DbSet<Account> Accounts { get; set; }

    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
        .HasKey(a => a.Id);

        modelBuilder.Entity<Account>()
      .OwnsMany(a => a.RefreshTokens, rt =>
      {
          rt.Property<int>("Id"); // Id property of RefreshToken
          rt.HasKey("Id");

          // Configure other properties of RefreshToken here
      });
       

       
    }
}