using Newtonsoft.Json;
// Concrete class implementing IDataStorage
public class JsonDataStorage : IDataStorage
{
    private string filePath = "students.json";

    public void SaveDataToJson(List<Student> students)
    {
        string jsonData = JsonConvert.SerializeObject(students, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);
    }

    public List<Student> LoadDataFromJson()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Student>>(jsonData);
        }

        return new List<Student>();
    }
}
