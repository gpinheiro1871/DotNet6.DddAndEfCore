using App.Infrastructure;
using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class StudentController
    {
        private SchoolContext _schoolContext;

        public StudentController(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
        }

        // EF Core Identity Map Pattern Comment
        // The FirstOrDefault method will get the record from the database and write it in the cache,
        // The Find method will look for the cache before resorting to the database and then write it in the cache
        // Allways use the Find method when retrieving a single entity.

        public string CheckStudentFavoriteCourse(long studentId, long courseId)
        {
			var student = _schoolContext.Students.Find(studentId);
			if (student is null)
                return "Student not found";

			var course = Course.FromId(courseId);
            if (course is null)
                return "Course not found";

            return student.FavoriteCourse == course ? "Yes" : "No";
        }

        public string AddEnrollment(long studentId, long courseId, Grade grade)
        {
            var student = _schoolContext.Students.Find(studentId);
            if (student is null)
                return "Student not found";

            var course = Course.FromId(courseId);
            if (course is null)
                return "Course not found";

            student.Enrollments.Add(new Enrollment(course, student, grade));

            _schoolContext.SaveChanges();

            return "OK";
        }
    }
}
