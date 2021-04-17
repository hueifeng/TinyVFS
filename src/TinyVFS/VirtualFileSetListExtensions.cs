using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using System.IO;
using System.Reflection;
using TinyVFS.Physical;

namespace TinyVFS
{
    public static class VirtualFileSetListExtensions
    {
        public static void AddEmbedded<T>(
            this VirtualFileSetList list,
            string baseNamespace = null,
            string baseFolder = null)
        {

            var assembly = typeof(T).Assembly;
            var fileProvider = CreateFileProvider(
                assembly,
                baseNamespace,
                baseFolder
            );

            list.Add(new EmbeddedVirtualFileSetInfo(fileProvider, assembly, baseFolder));
        }

        public static void AddPhysical(
             this VirtualFileSetList list,
             string root,
            ExclusionFilters exclusionFilters = ExclusionFilters.Sensitive)
        {
            var fileProvider = new PhysicalFileProvider(root, exclusionFilters);
            list.Add(new PhysicalVirtualFileSetInfo(fileProvider, root));
        }

        private static IFileProvider CreateFileProvider(
            Assembly assembly,
            string baseNamespace = null,
            string baseFolder = null)
        {

            var info = assembly.GetManifestResourceInfo("Microsoft.Extensions.FileProviders.Embedded.Manifest.xml");

            if (info == null)
            {
                return new Embedded.EmbeddedFileProvider(assembly, baseNamespace);
            }

            if (baseFolder == null)
            {
                return new ManifestEmbeddedFileProvider(assembly);
            }

            return new ManifestEmbeddedFileProvider(assembly, baseFolder);
        }

        public static void ReplaceEmbeddedByPhysical<T>(
             this VirtualFileSetList fileSets,
             string physicalPath)
        {

            var assembly = typeof(T).Assembly;

            for (var i = 0; i < fileSets.Count; i++)
            {
                if (fileSets[i] is EmbeddedVirtualFileSetInfo embeddedVirtualFileSet &&
                    embeddedVirtualFileSet.Assembly == assembly)
                {
                    var thisPath = physicalPath;

                    if (!embeddedVirtualFileSet.BaseFolder.IsNullOrEmpty())
                    {
                        thisPath = Path.Combine(thisPath, embeddedVirtualFileSet.BaseFolder);
                    }

                    fileSets[i] = new PhysicalVirtualFileSetInfo(new PhysicalFileProvider(thisPath), thisPath);
                }
            }
        }
    }
}
