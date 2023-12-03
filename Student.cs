
// Abstract class for Student inheriting from StudentName
public class Student : StudentName
{
    public string StudentID { get; set; }
    public string JoiningBatch { get; set; }
    public Department Department { get; set; }
    public Degree Degree { get; set; }
    public List<Semester> SemestersAttended { get; set; } = new List<Semester>();
}
