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

    // In a real world app, use DTO instead of accepting all these arguments separately
    public string RegisterStudent(string firstName, string lastName, 
        string email, long favoriteCourseId)
    {
        Course favoriteCourse = Course.FromId(favoriteCourseId);
        if (favoriteCourse is null)
            return "Course not found";

        Result<Email> emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        Result<Name> nameResult = Name.Create(firstName, lastName);
        if (nameResult.IsFailure)
        {
            return nameResult.Error;
        }

        var student = new Student(nameResult.Value, emailResult.Value, favoriteCourse);

        _studentRepository.Save(student);

        _schoolContext.SaveChanges();

        return "OK";
    }

    // Use a DTO!!
    public string EditPersonalInfo(long studentId, string firstName, 
        string lastName, string email, long favoriteCourseId)
    {
        Student? student = _studentRepository.GetById(studentId);
        if (student is null)
            return "Student not found";

        Course favoriteCourse = Course.FromId(favoriteCourseId);
        if (favoriteCourse is null)
            return "Course not found";

        Result<Email> emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        Result<Name> nameResult = Name.Create(firstName, lastName);
        if (nameResult.IsFailure)
        {
            return nameResult.Error;
        }

        student.Name = nameResult.Value;
        student.Email = emailResult.Value;
        student.FavoriteCourse = favoriteCourse;

        _schoolContext.SaveChanges();

        return "OK";
    }
}
