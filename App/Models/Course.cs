using App.Common;

namespace App.Models;

public class Course : Entity
{
    public static readonly Course English = new Course(1 , "English");
    public static readonly Course Mathematics = new Course(2 , "Mathematics");
    public static readonly Course Chemistry = new Course(3 , "Chemistry");

    public static readonly Course[] AllCourses = { English, Mathematics, Chemistry };

    public string Name { get; set; }

    protected Course()
    {

    }

    private Course(long id, string name) 
        : base(id)
    {
        Name = name;
    }

    public static Course? FromId(long id)
    {
        return AllCourses.SingleOrDefault(x => x.Id == id);
    }
}