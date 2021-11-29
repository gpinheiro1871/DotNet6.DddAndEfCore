using App.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

string connectionString = GetConnectionString();

using (var context = new SchoolContext(connectionString, true))
{
    var student = await context.Students
        .Include(x => x.FavoriteCourse)
        .FirstOrDefaultAsync(x => x.Id.Equals(1L));
}

string GetConnectionString()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration.GetConnectionString("Default");
}