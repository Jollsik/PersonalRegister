namespace PersonalRegister.Services
{
    public interface ICacheService
    {
        void AddToCache(string key, object value);
        object GetFromCache(string key);
        void RemoveFromCache(string key);
        //List<object> GetAll();
    }
}
