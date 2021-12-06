using App.Controllers;
using App.Infrastructure;
using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


//string result = Execute(x => x.CheckStudentFavoriteCourse(1, 1));
//string result2 = Execute(x => x.EnrollStudent(1, 1, Grade.A));
//string result3 = Execute(x => x.DisenrollStudent(1, 1));
//string result4 = Execute(x => x.RegisterStudent("Mary", "mary@gmail.com", 2));
string result5 = Execute(x => x.EditPersonalInfo(1, "john", "marston", 1L, "mary@gmail.com", 3));

//Console.WriteLine(result);
//Console.WriteLine(result2);
//Console.WriteLine(result3);
//Console.WriteLine(result4);
Console.WriteLine(result5);

string Execute(Func<StudentController, string> func)
{
    string connectionString = GetConnectionString();

    using (var context = new SchoolContext(connectionString, true))
    {
        var controller = new StudentController(context);
        return func(controller);
    }
}

string GetConnectionString()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration.GetConnectionString("Default");
}
