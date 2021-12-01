using App.Infrastructure;
using App.Models;

namespace App.Repositories
{
    public class StudentRepository
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public Student? GetById(long id)
        {
            Student? student = _context.Students.Find(id);

            if (student is null)
            {
                return null;
            }

            _context.Entry(student).Collection(x => x.Enrollments).Load();

            return student;
        }
    }
}
