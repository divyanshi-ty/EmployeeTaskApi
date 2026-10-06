using EmployeeTaskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Employee>(e =>
        {
            e.Property(x => x.FullName).HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasMaxLength(150).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Department).HasMaxLength(100).IsRequired();
            e.Property(x => x.Designation).HasMaxLength(100).IsRequired();
        });

        b.Entity<TaskItem>(t =>
        {
            t.Property(x => x.Title).HasMaxLength(200).IsRequired();
            t.Property(x => x.Description).HasMaxLength(1000);
            t.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            t.Property(x => x.Priority).HasConversion<string>().HasMaxLength(20);
            t.HasOne(x => x.Employee).WithMany(e => e.Tasks)
                .HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<AppUser>(u =>
        {
            u.Property(x => x.Username).HasMaxLength(50).IsRequired();
            u.HasIndex(x => x.Username).IsUnique();
            u.Property(x => x.Role).HasMaxLength(20).IsRequired();
        });
    }
}
