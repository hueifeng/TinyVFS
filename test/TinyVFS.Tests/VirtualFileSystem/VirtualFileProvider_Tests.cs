using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Shouldly;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace TinyVFS.Tests.VirtualFileSystem
{
    public class VirtualFileProvider_Tests
    {
        private readonly IVirtualFileProvider _virtualFileProvider;

        public VirtualFileProvider_Tests()
        {
            var services = new ServiceCollection();
            services.AddVirtualFilesService();
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<UnitTest1>(
                        baseFolder: "/MyResources"
                   );
            });
            var serviceProvider = services.BuildServiceProvider();
            _virtualFileProvider = serviceProvider.GetRequiredService<IVirtualFileProvider>();
        }

        [Fact]
        public void Should_Define_And_Get_Embedded_Resources()
        {
            //Act
            var resource = _virtualFileProvider.GetFileInfo("/js/jquery-3-1-1-min.js");

            //Assert
            resource.ShouldNotBeNull();
            resource.Exists.ShouldBeTrue();

            using (var stream = resource.CreateReadStream())
            {
                Encoding.UTF8.GetString(stream.GetAllBytes()).ShouldBe("//jquery-3-1-1-min.js-contents");
            }
        }

        [Fact]
        public void Should_Define_And_Get_Embedded_Resources_With_Special_Chars()
        {
            //Act
            var resource = _virtualFileProvider.GetFileInfo("/js/my{test}.2.9.min.js");

            //Assert
            resource.ShouldNotBeNull();
            resource.Exists.ShouldBeTrue();

            using (var stream = resource.CreateReadStream())
            {
                Encoding.UTF8.GetString(stream.GetAllBytes()).ShouldBe("//my{test}.2.9.min.js-content");
            }
        }

        [Fact]
        public void Should_Define_And_Get_Embedded_Directory_Contents()
        {
            //Act
            var contents = _virtualFileProvider.GetDirectoryContents("/js");

            //Assert
            contents.Exists.ShouldBeTrue();

            var contentList = contents.ToList();

            contentList.ShouldContain(x => x.Name == "jquery-3-1-1-min.js");
            contentList.ShouldContain(x => x.Name == "my{test}.2.9.min.js");
        }

        [Theory]
        [InlineData("/")]
        [InlineData("")]
        public void Should_Define_And_Get_Embedded_Root_Directory_Contents(string path)
        {
            //Act
            var contents = _virtualFileProvider.GetDirectoryContents(path);

            //Assert
            contents.Exists.ShouldBeTrue();

            var contentList = contents.ToList();
            contentList.ShouldContain(x => x.Name == "js");
        }
    }

    public static class StreamExtensions
    {
        public static byte[] GetAllBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Converts given string to a byte array using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        public static byte[] GetBytes(this string str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// Converts given string to a byte array using the given <paramref name="encoding"/>
        /// </summary>
        public static byte[] GetBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// Reads file content as string using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        public static string ReadAsString(this IFileInfo fileInfo)
        {
            return fileInfo.ReadAsString(Encoding.UTF8);
        }


        /// <summary>
        /// Reads file content as string using the given <paramref name="encoding"/>.
        /// </summary>
        public static string ReadAsString(this IFileInfo fileInfo, Encoding encoding)
        {
            using (var stream = fileInfo.CreateReadStream())
            {
                using (var streamReader = new StreamReader(stream, encoding, true))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }

}
