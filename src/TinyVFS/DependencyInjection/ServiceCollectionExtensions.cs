using TinyVFS;
using TinyVFS.VirtualFileSystem;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds virtual files service to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddVirtualFilesService(this IServiceCollection services)
        {
            services.AddSingleton<IWebContentFileProvider, WebContentFileProvider>();
            services.AddSingleton<IVirtualFileProvider, VirtualFileProvider>();
            services.AddSingleton<IDynamicFileProvider, DynamicFileProvider>();
            return services;
        }
    }
}
