using App.Common;

namespace App.Models;

// Lazy Loading Requirements: 
// - install Microsoft.EntityFrameworkCore.Proxies
// - non-sealed class
// - virtual navigation properties
// - pretected parameter-less constructor
// These requirements go against the separation of concerns, but
// they are small concessions that are worth it

public class Student : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public virtual Course FavoriteCourse { get; private set; }

    // Encapsulation comment
    // Introduce backing field to hold the actual list of Enrollments
    private readonly List<Enrollment> _enrollments = new List<Enrollment>();
    public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();

    protected Student()
    {

    }

    public Student(string name, string email, Course favoriteCourse)
    {
        Name = name;
        Email = email;
        FavoriteCourse = favoriteCourse;
    }

    public void EnrollIn(Course course, Grade grade)
    {
        var enrollment = new Enrollment(course, this, grade);

        _enrollments.Add(enrollment);
    }
}
