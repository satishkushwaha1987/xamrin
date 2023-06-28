namespace CHSBackOffice.Droid.InterfacesImplementation
{
    class SqLiteImp : Database.ISqLite
    {
        public string GetPathToDatabase(string fileName)
        {
            var path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);
            return path;
        }
    }
}