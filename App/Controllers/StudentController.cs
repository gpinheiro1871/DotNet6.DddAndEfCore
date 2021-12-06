using App.Infrastructure;
using App.Models;
using App.Repositories;
using CSharpFunctionalExtensions;

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
        Student? student = _studentRepository.GetById(studentId);
        if (student is null)
            return "Student not found";

        Course course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        return student.FavoriteCourse == course ? "Yes" : "No";
    }

    public string EnrollStudent(long studentId, long courseId, Grade grade)
    {
        Student? student = _studentRepository.GetById(studentId);
        if (student is null)
            return "Student not found";

        Course course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        //student.Enrollments.Add(new Enrollment(course, student, grade));
        string result = student.EnrollIn(course, grade);

        _schoolContext.SaveChanges();

        return result;
    }

    public string DisenrollStudent(long studentId, long courseId)
    {
        Student? student = _studentRepository.GetById(studentId);
        if (student is null)
            return "Student not found";

        Course course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        student.Disenroll(course);

        _schoolContext.SaveChanges();

        return "OK";
    }

    public string RegisterStudent(string name, string email, long favoriteCourseId)
    {
        Course favoriteCourse = Course.FromId(favoriteCourseId);
        if (favoriteCourse is null)
            return "Course not found";

        Result<Email> result = Email.Create(email);
        if (result.IsFailure)
        {
            return result.Error;
        }

        //var student = new Student(name, result.Value, favoriteCourse);
        
        //_studentRepository.Save(student);

        _schoolContext.SaveChanges();

        return "OK";
    }

    public string EditPersonalInfo(long studentId, string name, string email, long favoriteCourseId)
    {
        Student? student = _studentRepository.GetById(studentId);
        if (student is null)
            return "Student not found";

        Course favoriteCourse = Course.FromId(favoriteCourseId);
        if (favoriteCourse is null)
            return "Course not found";

        Result<Email> result = Email.Create(email);
        if (result.IsFailure)
        {
            return result.Error;
        }


        //student.Name = name;
        student.Email = result.Value;
        student.FavoriteCourse = favoriteCourse;   

        _schoolContext.SaveChanges();

        return "OK";
    }
}
