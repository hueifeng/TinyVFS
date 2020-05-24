using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;

namespace TinyVFS.VirtualFileSystem
{
    public class RazorViewEngineVirtualFileProvider : IFileProvider
    {
        private readonly Lazy<IFileProvider> _fileProvider;
        private readonly IServiceProvider _serviceProviderAccessor;

        public RazorViewEngineVirtualFileProvider(IServiceProvider serviceProviderAccessor)
        {
            _serviceProviderAccessor = serviceProviderAccessor;
            _fileProvider = new Lazy<IFileProvider>(
                () => serviceProviderAccessor.GetRequiredService<IVirtualFileProvider>(),
                true
            );
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (!IsInitialized())
            {
                return new NotFoundFileInfo(subpath);
            }

            return _fileProvider.Value.GetFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            if (!IsInitialized())
            {
                return new NotFoundDirectoryContents();
            }

            return _fileProvider.Value.GetDirectoryContents(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            if (!IsInitialized())
            {
                return NullChangeToken.Singleton;
            }

            return _fileProvider.Value.Watch(filter);
        }

        private bool IsInitialized()
        {
            return _serviceProviderAccessor != null;
        }
    }
}