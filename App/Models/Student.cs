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
    public virtual Name Name { get; set; }
    public Email Email { get; set; }
    public virtual Course FavoriteCourse { get; set; }

    // Encapsulation comment
    // Introduce backing field to hold the actual list of Enrollments
    private readonly List<Enrollment> _enrollments = new List<Enrollment>();
    public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();

    protected Student()
    {

    }

    public Student(Name name, Email email, Course favoriteCourse)
    {
        Name = name;
        Email = email;
        FavoriteCourse = favoriteCourse;
    }

    public string EnrollIn(Course course, Grade grade)
    {
        if (_enrollments.Any(x => x.Course == course))
        {
            return $"Already enrolled in course '{course.Name}'";
        }

        var enrollment = new Enrollment(course, this, grade);

        _enrollments.Add(enrollment);

        return "OK";
    }

    public void Disenroll(Course course)
    {
        Enrollment? enrollment = _enrollments.FirstOrDefault(x => x.Course == course);
        if (enrollment is null)
            return;

        _enrollments.Remove(enrollment);
    }
}
