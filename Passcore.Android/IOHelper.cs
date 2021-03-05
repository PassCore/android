using System.IO;

namespace Passcore.Android
{
    class IOHelper
    {
        public static string GetDataPath()
        => Xamarin.Essentials.FileSystem.AppDataDirectory;

        public static void WriteAllLines(string file, string[] content)
        => File.WriteAllLines(Path.Combine(GetDataPath(), file), content);

        public static void WriteAllText(string file, string content)
        => File.WriteAllText(Path.Combine(GetDataPath(), file), content);

        public static string ReadAllText(string file)
        {
            var path = Path.Combine(GetDataPath(), file);
            if (!File.Exists(path))
                return null;
            return File.ReadAllText(path);
        }
    }
}