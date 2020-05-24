using TinyVFS;
using TinyVFS.VirtualFileSystem;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        ///     
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddVirtualFilesService(this IServiceCollection services)
        {
            services.AddSingleton<IWebContentFileProvider, WebContentFileProvider>();
            services.AddSingleton<IVirtualFileProvider, VirtualFileProvider>();
            services.AddSingleton<IDynamicFileProvider, DynamicFileProvider>();
            return services;
        }
    }
}
