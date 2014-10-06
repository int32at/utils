using System.IO;
using System.Text;

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

        public static FileInfo Ensure(this FileInfo info)
        {
            if (!info.Exists)
                info.Create();

            return info;
        }

        public static string ReadAlltext(this FileInfo file)
        {
            var builder = new StringBuilder();

            using (var reader = new StreamReader(
                File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                while (reader.Peek() >= 0)
                    builder.AppendLine(reader.ReadLine());
            }

            return builder.ToString();
        }

        public static void WriteAllText(this FileInfo file, string contents)
        {
            File.WriteAllText(file.ThrowIfNull("file").FullName, contents);
        }
    }
}
