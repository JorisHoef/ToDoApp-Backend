namespace ToDoAppBackend.Services
{
    public class LinkCreator
    {
        /// <summary>
        /// Creates HTML link, which seems unnecessary since I don't want this backend to decide what the frontend should look like
        /// </summary>
        /// <param name="id"></param>
        /// <param name="displayName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>MIGHT want to use this, in case the Name should be shown before making the call to the object itself</remarks>
        public string CreateHtmlLink<T>(long id, string displayName)
        {
            var typeName = typeof(T).Name.ToLower();
            return $"<a href='/{typeName}s/{id}'>{displayName}</a>";
        }
        
        /// <summary>
        /// Just create the link based on type name
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string CreateLink<T>(long id)
        {
            var typeName = typeof(T).Name.ToLower();
            return $"/{typeName}s/{id}";
        }
    }
}