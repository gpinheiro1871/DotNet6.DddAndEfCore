using App.Infrastructure;
using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

string connectionString = GetConnectionString();

// EF Core Identity Map Pattern Comment
// The FirstOrDefault method will get the record from the database and write it in the cache,
// The Find method will look for the cache before resorting to the database and then write it in the cache
// Allways use the Find method when retrieving a single entity.

using (var context = new SchoolContext(connectionString, true))
{
    var student = context.Students.Find(1L);

    var course = student.FavoriteCourse;

    var course2 = context.Courses.SingleOrDefault(x => x.Id == 1L);

    bool coursesEqual = course == course2;

    bool courseIssue = course == Course.English;
}

string GetConnectionString()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration.GetConnectionString("Default");
}