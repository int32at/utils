namespace int32.Utils.Windows.Files.Links
{
    public class Link
    {
        public string Path { get; set; }

        public string Resolve()
        {
            return new LinkResolver().Resolve(Path);
        }
    }
}
