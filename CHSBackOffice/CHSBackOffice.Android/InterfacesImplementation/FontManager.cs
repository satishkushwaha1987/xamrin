
using System;
using System.IO;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Interfaces;

namespace CHSBackOffice.Droid.InterfacesImplementation
{
    public class FontManager : IFontManager
    {
        public string GetFontFilePath(string fileName)
        {
            try 
            {
                string fontPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);

                using (var asset = MainActivity.AssetManager.Open(fileName))
                using (var dest = File.Open(fontPath, FileMode.Create))
                {
                    asset.CopyTo(dest);
                }
                return fontPath;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return string.Empty;
            }
            
        }
    }
}