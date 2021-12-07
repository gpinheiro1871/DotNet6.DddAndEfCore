using App.Common;
using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure;

public sealed class SchoolContext : DbContext
{
    private static readonly Type[] EnumerationTypes = { typeof(Course), typeof(Suffix) };

    private readonly string? _connectionString;
    private readonly bool _useConsoleLogger;
    private readonly EventDispatcher _eventDispatcher;

    public DbSet<Student> Students { get; set; }

    public SchoolContext()
    {

    }

    public SchoolContext(string connectionString, bool useConsoleLogger, EventDispatcher eventDispatcher)
    {
        _connectionString = connectionString;
        _useConsoleLogger = useConsoleLogger;
        _eventDispatcher = eventDispatcher;
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

        optionsBuilder.UseLazyLoadingProxies();

        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(x =>
        {
            x.ToTable("Student").HasKey(y => y.Id);

            x.HasOne(x => x.FavoriteCourse).WithMany();

            x.Property(x => x.Email)
                .HasConversion(y => y.Value, y => Email.Create(y).Value);

            x.OwnsOne(y => y.Name, y =>
            {
                y.Property(z => z.First).HasColumnName("Firstname");
                y.Property(z => z.Last).HasColumnName("Lastname");

                y.Property<long?>("NameSuffixId").HasColumnName("NameSuffixId");

                y.HasOne(z => z.Suffix)
                    .WithMany()
                    .HasForeignKey("NameSuffixId")
                    .IsRequired(false);

            });

            x.HasMany(x => x.Enrollments).WithOne(y => y.Student)
                .Metadata.PrincipalToDependent?.SetPropertyAccessMode(PropertyAccessMode.Field); //Assign backing field manually (EFCore does this automatically)
        });

        modelBuilder.Entity<Suffix>(x => 
        {
            x.ToTable("Suffix").HasKey(y => y.Id);
            
        });

        modelBuilder.Entity<Course>(x =>
        {
            x.ToTable("Course").HasKey(y => y.Id);
        }); 
        
        modelBuilder.Entity<Enrollment>(x =>
        {
            x.ToTable("Enrollment").HasKey(y => y.Id);

            x.HasOne(y => y.Student).WithMany(y => y.Enrollments);

            x.HasOne(y => y.Course).WithMany();

            x.Property(p => p.Grade);
        });
    }

    public override int SaveChanges()
    {
        // Add reference data here
        // This is because EFCore recognizes that the reference data state was changed (Too bad!),
        // So we need to do this for each enumeration class in the domain
        //foreach (EntityEntry<Course> course in ChangeTracker.Entries<Course>())
        //{
        //    course.State = EntityState.Unchanged;
        //}

        //foreach (EntityEntry<Suffix> suffix in ChangeTracker.Entries<Suffix>())
        //{
        //    suffix.State = EntityState.Unchanged;
        //}

        // Better solution:
        IEnumerable<EntityEntry> enumerationEntries = ChangeTracker.Entries()
            .Where(x => EnumerationTypes.Contains(x.Entity.GetType()));

        foreach (EntityEntry enumerationEntry in enumerationEntries)
        {
            enumerationEntry.State = EntityState.Unchanged;
        }

        List<Entity> entities = ChangeTracker
            .Entries()
            .Where(x => x.Entity is Entity)
            .Select(x => (Entity)x.Entity)
            .ToList();

        int result = base.SaveChanges();

        foreach (Entity entity in entities)
        {
            // dispatch events
            _eventDispatcher.Dispatch(entity.DomainEvents);

            // clear all events
            entity.ClearDomainEvents();
        }

        return result;
    }
}
