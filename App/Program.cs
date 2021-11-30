using App.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

string connectionString = GetConnectionString();

// EF Core Identity Map Pattern Comment
// The FirstOrDefault method will get the record from the database and write it in the cache,
// The Find method will look for the cache before resorting to the database and then write it in the cache
// Allways use the Find method when retrieving a single entity.

using (var context = new SchoolContext(connectionString, true))
{
    var student = await context.Students
        .FirstOrDefaultAsync(x => x.Id.Equals(1L));

    var course = context.Courses.Find(1L);

    var student2 = context.Students.Find(1L);

    //var student3 = context.Students.FirstOrDefaultAsync(x => x.Id.Equals(1L)); //Too Bad!

    bool referenceEquals = object.ReferenceEquals(student, student2); // True
    bool equals2 = object.ReferenceEquals(course, student2.FavoriteCourse); // True
}

string GetConnectionString()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration.GetConnectionString("Default");
}