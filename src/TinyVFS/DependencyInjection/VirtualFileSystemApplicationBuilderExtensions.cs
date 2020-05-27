using System;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using TinyVFS.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    ///     Extends <see cref="IApplicationBuilder"/> with virtual file system configuration methods.
    /// </summary>
    public static class VirtualFileSystemApplicationBuilderExtensions
    {

        /// <summary>
        /// Adds a middleware that provides virtual files system.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>A reference to the <paramref name="app"/> after the operation has completed.</returns>
        public static void UseVirtualFiles(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = app.ApplicationServices.GetRequiredService<IWebContentFileProvider>()
                }
            );
        }

        /// <summary>
        /// Adds a middleware that provides virtual files system.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name = "options" > A <see cref="StaticFileOptions"/> used to configure the middleware.</param>
        /// <returns>A reference to the <paramref name="app"/> after the operation has completed.</returns>
        public static void UseVirtualFiles(this IApplicationBuilder app, StaticFileOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.UseStaticFiles(options);
        }

        /// <summary>
        /// Adds a middleware that provides virtual files system.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name = "contentTypeProvider" > A <see cref="IContentTypeProvider"/> used to configure the middleware.</param>
        /// <returns>A reference to the <paramref name="app"/> after the operation has completed.</returns>
        public static void UseVirtualFiles(this IApplicationBuilder app, IContentTypeProvider contentTypeProvider)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (contentTypeProvider == null)
            {
                throw new ArgumentNullException(nameof(contentTypeProvider));
            }


            app.UseStaticFiles(
                new StaticFileOptions
                {
                    ContentTypeProvider = contentTypeProvider,
                    FileProvider = app.ApplicationServices.GetRequiredService<IWebContentFileProvider>()
                }
            );
        }


    }
}
