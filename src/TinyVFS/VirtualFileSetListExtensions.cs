using System.IO;
using System.Linq;
using TinyVFS.Embedded;

namespace TinyVFS
{
    public static class VirtualFileSetListExtensions
    {
        public static void AddEmbedded<T>(this VirtualFileSetList list, string baseNamespace = null, string baseFolderInProject = null)
        {
            list.Add(
                new EmbeddedFileSet(
                    typeof(T).Assembly,
                    baseNamespace,
                    baseFolderInProject
                )
            );
        }
        
        public static void ReplaceEmbeddedByPhysical<T>(this VirtualFileSetList list, string pyhsicalPath)
        {
            var assembly = typeof(T).Assembly;
            var embeddedFileSets = list.OfType<EmbeddedFileSet>().Where(fs => fs.Assembly == assembly).ToList();

            foreach (var embeddedFileSet in embeddedFileSets)
            {
                list.Remove(embeddedFileSet);

                if (!embeddedFileSet.BaseFolderInProject.IsNullOrEmpty())
                {
                    pyhsicalPath = Path.Combine(pyhsicalPath, embeddedFileSet.BaseFolderInProject);
                }

                list.PhysicalPaths.Add(pyhsicalPath);
            }
        }
    }
}
