using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TinyVFS
{
    public class EmbeddedVirtualFileSetInfo : VirtualFileSetInfo
    {
        public Assembly Assembly { get; }

        public string BaseFolder { get; }

        public EmbeddedVirtualFileSetInfo(
            IFileProvider fileProvider,
            Assembly assembly,
            string baseFolder = null)
            : base(fileProvider)
        {
            Assembly = assembly;
            BaseFolder = baseFolder;
        }
    }
}
