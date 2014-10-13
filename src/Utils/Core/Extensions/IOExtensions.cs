using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using int32.Utils.Windows.Files.Links;

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

        public static FileInfo Copy(this FileInfo info, string destFileNameWithoutExtension)
        {
            return info.Copy(destFileNameWithoutExtension, false);
        }

        public static FileInfo Copy(this FileInfo info, string destFileNameWithoutExtension, bool overwrite)
        {
            var path = info.DirectoryName;
            return info.CopyTo(Path.Combine(path, destFileNameWithoutExtension) + info.Extension);
        }

        public static DirectoryInfo ToDirectoryInfo(this Environment.SpecialFolder folder)
        {
            return new DirectoryInfo(Environment.GetFolderPath(folder));
        }


        //related to Windows.Files

        public static IEnumerable<string> ResolveAll(this IEnumerable<Link> links)
        {
            var resolver = new LinkResolver();
            var list = new List<string>();

            links.ForEach(i => list.Add(resolver.Resolve(i.Path)));

            return list;
        }
    }
}
