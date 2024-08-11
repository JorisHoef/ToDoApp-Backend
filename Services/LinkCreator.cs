namespace ToDoAppBackend.Services
{
    public class LinkCreator
    {
        public string CreateLink<T>(long id, string displayName)
        {
            var typeName = typeof(T).Name.ToLower();
            return $"<a href='/{typeName}s/{id}'>{displayName}</a>";
        }
    }
}