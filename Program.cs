using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    private static List<Student> students;
    private static IDataStorage dataStorage;

    // Hardcoded list of courses
    private static List<Course> allCourses = new List<Course>
    {
        new Course { CourseID = "CSC101", CourseName = "Introduction to Programming", InstructorName = "John Doe", Credits = 3 },
        new Course { CourseID = "MAT102", CourseName = "Calculus I", InstructorName = "Jane Smith", Credits = 4 },
        new Course { CourseID = "ENG103", CourseName = "English Composition", InstructorName = "Alice Johnson", Credits = 3 },
        
    };

    static Program()
    {
        // Initialize the data storage
        dataStorage = new JsonDataStorage();
        // Load existing data from JSON
        students = dataStorage.LoadDataFromJson();
    }

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Add New Student");
            Console.WriteLine("2. View Student Details");
            Console.WriteLine("3. Delete Student");
            Console.WriteLine("4. Add New Semester");
            Console.WriteLine("5. Exit\n");

            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        AddNewStudent();
                        break;
                    case 2:
                        ViewStudentDetails();
                        break;
                    case 3:
                        DeleteStudent();
                        break;
                    case 4:
                        AddNewSemester();
                        break;
                    
                    case 5:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }

    static void AddNewSemester()
    {
        Console.Write("Enter Student ID: ");
        string studentID = Console.ReadLine();

        Student student = students.Find(s => s.StudentID == studentID);

        if (student != null)
        {
            Semester newSemester = new Semester();

            Console.Write("Enter Semester Code (0: Summer, 1: Fall, 2: Spring): ");
            newSemester.SemesterCode = (SemesterCode)Enum.Parse(typeof(SemesterCode), Console.ReadLine());
            Console.Write("Enter Year: ");
            newSemester.Year = Console.ReadLine();

            // Show courses the student hasn't taken yet
            var coursesNotTaken = allCourses.Except(student.SemestersAttended.SelectMany(s => s.Courses));

            Console.WriteLine("Courses Not Taken:");
            foreach (var course in coursesNotTaken)
            {
                Console.WriteLine($"{course.CourseID} - {course.CourseName} ({course.Credits} credits)");
            }

            // Let the user select courses
            Console.Write("Enter Course ID to add (comma-separated for multiple): ");
            string[] selectedCourseIDs = Console.ReadLine().Split(',');

            foreach (var courseID in selectedCourseIDs)
            {
                Course selectedCourse = allCourses.Find(c => c.CourseID.Trim() == courseID.Trim());
                if (selectedCourse != null)
                {
                    newSemester.Courses.Add(selectedCourse);
                }
                else
                {
                    Console.WriteLine($"Course with ID {courseID} not found.");
                }
            }

            student.SemestersAttended.Add(newSemester);

            // Save data after adding a new semester
            dataStorage.SaveDataToJson(students);

            Console.WriteLine("Semester added successfully.");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }

    static void AddNewStudent()
    {
        Student newStudent = new Student();

        Console.Write("Enter First Name: ");
        newStudent.FirstName = Console.ReadLine();
        Console.Write("Enter Middle Name: ");
        newStudent.MiddleName = Console.ReadLine();
        Console.Write("Enter Last Name: ");
        newStudent.LastName = Console.ReadLine();
        Console.Write("Enter Student ID(XXX-XXX-XXX): ");
        newStudent.StudentID = Console.ReadLine();
        Console.Write("Enter Joining Batch: ");
        newStudent.JoiningBatch = Console.ReadLine();
        Console.Write("Enter Department (0: ComputerScience, 1: BBA, 2: English): ");
        newStudent.Department = (Department)Enum.Parse(typeof(Department), Console.ReadLine());
        Console.Write("Enter Degree (0: BSC, 1: BBA, 2: BA, 3: MSC, 4: MBA, 5: MA): ");
        newStudent.Degree = (Degree)Enum.Parse(typeof(Degree), Console.ReadLine());

        students.Add(newStudent);

        // Save data after adding a new student
        dataStorage.SaveDataToJson(students);
    }

    static void ViewStudentDetails()
    {
       
        Console.Write("Enter Student ID: ");
        string studentID = Console.ReadLine();

        Student student = students.Find(s => s.StudentID == studentID);
        Console.WriteLine("\n*****************************\n");
        if (student != null)
        {
            Console.WriteLine($"Name: {student.FirstName} {student.MiddleName} {student.LastName}");
            Console.WriteLine($"Student ID: {student.StudentID}");
            Console.WriteLine($"Joining Batch: {student.JoiningBatch}");
            Console.WriteLine($"Department: {student.Department}");
            Console.WriteLine($"Degree: {student.Degree}");
            Console.WriteLine("Semesters Attended:\n");

            foreach (var semester in student.SemestersAttended)
            {
                Console.WriteLine($"  {semester.SemesterCode} {semester.Year}");
                Console.WriteLine("  Courses:");

                foreach (var course in semester.Courses)
                {
                    Console.WriteLine($"    {course.CourseID} - {course.CourseName} ({course.Credits} credits)");
                }
            }
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
        Console.WriteLine("*****************************\n");

    }

    static void DeleteStudent()
    {
        Console.Write("Enter Student ID to delete: ");
        string studentID = Console.ReadLine();

        Student student = students.Find(s => s.StudentID == studentID);

        if (student != null)
        {
            students.Remove(student);
            Console.WriteLine($"Student {studentID} and corresponding dependencies deleted.");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }

        // Save data after deleting a student
        dataStorage.SaveDataToJson(students);
    }

    static void SaveDataToJson()
    {
        // Save students data to JSON file
        dataStorage.SaveDataToJson(students);
        Console.WriteLine("Data saved to students.json");
    }
}
