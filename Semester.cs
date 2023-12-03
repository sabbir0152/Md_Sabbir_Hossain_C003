//semester details
public class Semester
{
    public SemesterCode SemesterCode { get; set; }
    public string Year { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();
}
