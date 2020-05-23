using Microsoft.Extensions.FileProviders;

namespace TinyVFS
{
    public interface IDynamicFileProvider:IFileProvider
    {

        /// <summary>
        ///     Add Or Update File
        /// </summary>
        /// <param name="fileInfo"></param>
        void AddOrUpdate(IFileProvider fileInfo);

        /// <summary>
        ///     Delete File
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool Delete(string filePath);

    }
}
