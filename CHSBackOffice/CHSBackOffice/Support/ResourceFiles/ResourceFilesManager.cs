using CHSBackOffice.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CHSBackOffice.Support.ResourceFiles
{
    /// <summary>
    /// Helps to retrieve content from text file embedded to app resources.
    /// All resource files should be located in Resources/ResourceFiles folder
    /// </summary>
    public sealed class ResourceFilesManager
    {
        private static Dictionary<string, string> _resourceFilesContent = new Dictionary<string, string>();

        /// <summary>
        /// Retrieve content from text file embedded to app resources.
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns>File Content</returns>
        public static string GetFileContent(string fileName)
        {
            if (_resourceFilesContent.ContainsKey(fileName))
                return _resourceFilesContent[fileName];

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Resource)).Assembly;
            var assemblyName = assembly.FullName.Split(',').First();
            Stream stream = assembly.GetManifestResourceStream($"{assemblyName}.Resources.ResourceFiles.{fileName}");

            string fileContent = "";
            if (stream != null)
                using (var reader = new StreamReader(stream))
                {
                    fileContent = reader.ReadToEnd();
                }

            _resourceFilesContent.Add(fileName, fileContent);
            return fileContent;
        }
    }
}
