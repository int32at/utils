using System.Collections.Generic;
using System.IO;
using System.Linq;
using int32.Utils.Windows.Files.Links;

namespace int32.Utils.Windows.Files
{
    public static class Finder
    {
        public static IEnumerable<Link> GetLinks(string path)
        {
            return Directory.GetFiles(path, "*.lnk", SearchOption.AllDirectories).Select(i => new Link {Path = i});
        } 
    }
}
