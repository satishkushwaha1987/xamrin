using System;
using System.IO;

namespace CHSBackOffice.iOS.InterfacesImplementation
{
    class SqLiteImp : Database.ISqLite
    {
        public string GetPathToDatabase(string fileName)
        {
            var docs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(docs, "..", "Library", fileName);
            return path;
        }
    }
}