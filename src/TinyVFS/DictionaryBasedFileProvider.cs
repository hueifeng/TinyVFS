using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace TinyVFS
{
    public abstract class DictionaryBasedFileProvider : IFileProvider
    {
        protected abstract IDictionary<string, IFileInfo> Files { get; }

        public virtual IDirectoryContents GetDirectoryContents(string subpath)
        {
            var directory = GetFileInfo(subpath);
            if (!directory.IsDirectory)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            var fileList = new List<IFileInfo>();

            var directoryPath = subpath.EnsureEndsWith('/');
            foreach (var fileInfo in Files.Values)
            {
                var fullPath = fileInfo.GetVirtualOrPhysicalPathOrNull();
                if (!fullPath.StartsWith(directoryPath))
                {
                    continue;
                }

                var relativePath = fullPath.Substring(directoryPath.Length);
                if (relativePath.Contains("/"))
                {
                    continue;
                }

                fileList.Add(fileInfo);
            }
            return new EnumerableDirectoryContents(fileList);

        }

        /// <summary>Locate a file at the given path.</summary>
        /// <param name="subpath">Relative path that identifies the file.</param>
        /// <returns>The file information. Caller must check Exists property.</returns>
        public virtual IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            var file = Files.GetOrDefault(NormalizePath(subpath));

            if (file == null)
            {
                return new NotFoundFileInfo(subpath);
            }

            return file;
        }

        public virtual IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        protected virtual string NormalizePath(string subpath)
        {
            return subpath;
        }
    }
}
