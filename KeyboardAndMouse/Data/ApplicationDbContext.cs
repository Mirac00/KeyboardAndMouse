using KeyboardAndMouse.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeyboardAndMouse.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Keyboard> Keyboards { get; set; }
    public DbSet<Mouse> Mice { get; set; }
    public DbSet<Set> Sets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        });

        modelBuilder.Entity<Set>()
            .HasOne(s => s.Keyboard)
            .WithMany(k => k.Sets)
            .HasForeignKey(s => s.KeyboardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Set>()
            .HasOne(s => s.Mouse)
            .WithMany(m => m.Sets)
            .HasForeignKey(s => s.MouseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Set>()
            .HasOne(s => s.Category)
            .WithMany(c => c.Sets)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
