using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace TinyVFS.Embedded
{
    /// <summary>
    /// Represents a file embedded in an assembly.
    /// </summary>
    public class EmbeddedResourceFileInfo : IFileProvider
    {

        public bool Exists => true;

        public long Length
        {
            get
            {
                if (!_length.HasValue)
                {
                    using (var stream=_assembly.GetManifestResourceStream(_resourcePath))
                    {
                        _length = stream.Length;
                    }
                }

                return _length.Value;
            }
        }

        private long? _length;

        public string PhysicalPath => null;

        public string VirtualPath { get; }

        /// <summary>
        /// The time, in UTC.
        /// </summary>
        public DateTimeOffset LastModified { get; }

        public bool IsDirectory => false;

        public string Name { get; }
        private readonly string _resourcePath;

        public IFileInfo GetFileInfo(string subpath)
        {
            throw new NotImplementedException();
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }

        private readonly Assembly _assembly;

        public EmbeddedResourceFileInfo(
            Assembly assembly,
            string resourcePath,
            string virtualPath,
            string name,
            DateTimeOffset lastModified)
        {
            _assembly = assembly;
            _resourcePath = resourcePath;

            VirtualPath = virtualPath;
            Name = name;
            LastModified = lastModified;
        }

        public Stream CreateReadStream()
        {
            throw  new NotImplementedException();
        }
    }
}
