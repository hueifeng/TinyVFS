using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

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

            var fileList=new List<IFileInfo>();

            var directoryPath = subpath.EnsureEndsWith('/');
            foreach (var fileInfo in Files.Values)
            {
                var fullPath=fileInfo.
            }

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
