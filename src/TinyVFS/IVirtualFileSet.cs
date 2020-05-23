using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;

namespace TinyVFS
{
    public interface IVirtualFileSet
    {
        /// <summary>
        ///     Add files
        /// </summary>
        /// <param name="files"></param>
        void AddFiles(Dictionary<string, IFileInfo> files);

    }
}
