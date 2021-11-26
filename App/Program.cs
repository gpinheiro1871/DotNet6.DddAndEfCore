using App.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

string connectionString = GetConnectionString();

using (var context = new SchoolContext(connectionString, true))
{
    var student = context.Students.Find(1L);
}

string GetConnectionString()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration.GetConnectionString("Default");
}