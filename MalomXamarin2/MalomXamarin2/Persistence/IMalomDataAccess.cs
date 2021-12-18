namespace MalomXamarin2.Persistence
{
    public interface IMalomDataAccess
    {
        MalomBoard Load(string path);

        void Save(string path, MalomBoard board);
    }
}
