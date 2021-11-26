using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure;

public sealed class SchoolContext : DbContext
{
    private readonly string? _connectionString;
    private readonly bool _useConsoleLogger;

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    public SchoolContext(string? connectionString = null, bool useConsoleLogger = false)
    {
        _connectionString = connectionString;
        _useConsoleLogger = useConsoleLogger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
            .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
            .AddConsole();
        });

        if (_connectionString is null)
        {
            optionsBuilder
                .UseSqlite();
        }
        else
        {
            optionsBuilder
                .UseSqlite(_connectionString);
        }

        if (_useConsoleLogger)
        {
            optionsBuilder
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(x =>
        {
            x.ToTable("Student").HasKey(y => y.Id);
        });

        modelBuilder.Entity<Course>(x =>
        {
            x.ToTable("Course").HasKey(y => y.Id);
        });
    }
}
