namespace MedCon.Services
{
	public interface IStorage
    {
        string Get(string key);
        void Set(string key, string obj);
        void Remove(string key);
    }
}
