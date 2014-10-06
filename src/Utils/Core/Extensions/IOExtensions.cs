using System.IO;

namespace int32.Utils.Core.Extensions
{
    public static class IOExtensions
    {
        public static DirectoryInfo Ensure(this DirectoryInfo info)
        {
            if (!info.Exists)
                info.Create();

            return info;
        }

        public static string ReadAlltext(this FileInfo file)
        {
            return File.ReadAllText(file.ThrowIfNull("file").FullName);
        }
    }
}
