using App.Infrastructure;
using App.Models;

namespace App.Repositories
{
    public sealed class StudentRepository
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

        public void Save(Student student)
        {
            // Use attach instead
            // Otherwise, EF will try to add the course again, too bad!
            //_schoolContext.Students.Add(student);

            // Attach marks only entities with no id as new

            _context.Attach(student);
        }
    }
}
