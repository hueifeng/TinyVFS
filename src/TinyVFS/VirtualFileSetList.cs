using System.Collections.Generic;

namespace TinyVFS
{
    public class VirtualFileSetList:List<VirtualFileSetInfo>
    {
        public List<string> PhysicalPaths { get; }

        public VirtualFileSetList() {
            PhysicalPaths = new List<string>();
        }
    }
}
