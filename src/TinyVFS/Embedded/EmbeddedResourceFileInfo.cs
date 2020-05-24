using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Reflection;

namespace TinyVFS.Embedded
{
    /// <summary>
    /// Represents a file embedded in an assembly.
    /// </summary>
    public class EmbeddedResourceFileInfo : IFileInfo
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
            var stream = _assembly.GetManifestResourceStream(_resourcePath);

            if (!_length.HasValue && stream != null)
            {
                _length = stream.Length;
            }

            return stream;
        }

        public override string ToString()
        {
            return $"[EmbeddedResourceFileInfo] {Name} ({this.VirtualPath})";
        }
    }
}
