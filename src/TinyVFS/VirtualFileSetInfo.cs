using Microsoft.Extensions.FileProviders;

namespace TinyVFS
{
    public class VirtualFileSetInfo
    {
        public IFileProvider FileProvider { get; }

        public VirtualFileSetInfo(IFileProvider fileProvider)
        {
            FileProvider =fileProvider;
        }
    }
}
