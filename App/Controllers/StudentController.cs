using App.Infrastructure;
using App.Models;
using App.Repositories;

namespace App.Controllers;

public class StudentController
{
    private readonly StudentRepository _studentRepository;
    private readonly SchoolContext _schoolContext;

    public StudentController(SchoolContext schoolContext)
    {
        _studentRepository = new StudentRepository(schoolContext);
        _schoolContext = schoolContext;
    }

    // EF Core Identity Map Pattern Comment
    // The FirstOrDefault method will get the record from the database and write it in the cache,
    // The Find method will look for the cache before resorting to the database and then write it in the cache
    // Allways use the Find method when retrieving a single entity.

    public string CheckStudentFavoriteCourse(long studentId, long courseId)
    {
        var student = _studentRepository.GetById(studentId);
        if (student is null)
            return "Student not found";

        var course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        return student.FavoriteCourse == course ? "Yes" : "No";
    }

    public string EnrollStudent(long studentId, long courseId, Grade grade)
    {
        var student = _studentRepository.GetById(studentId);
        if (student is null)
            return "Student not found";

        var course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        //student.Enrollments.Add(new Enrollment(course, student, grade));
        var result = student.EnrollIn(course, grade);

        _schoolContext.SaveChanges();

        return result;
    }
}
