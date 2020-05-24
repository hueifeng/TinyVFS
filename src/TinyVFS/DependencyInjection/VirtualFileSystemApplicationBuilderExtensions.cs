using Microsoft.Extensions.DependencyInjection;
using TinyVFS.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder
{
    public static class VirtualFileSystemApplicationBuilderExtensions
    {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="app"></param>
        public static void UseVirtualFiles(this IApplicationBuilder app)
        {
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = app.ApplicationServices.GetRequiredService<IWebContentFileProvider>()
                }
            );
        }
    }
}
