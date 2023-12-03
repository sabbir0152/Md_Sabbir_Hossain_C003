
// Interface for data storage

public interface IDataStorage
{
    void SaveDataToJson(List<Student> students);
    List<Student> LoadDataFromJson();
}
