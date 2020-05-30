# TinyVFS

[![nuget](https://img.shields.io/nuget/v/TinyVFS.svg?style=flat-square)](https://www.nuget.org/packages/TinyVFS) [![stats](https://img.shields.io/nuget/dt/TinyVFS.svg?style=flat-square)](https://www.nuget.org/stats/packages/TinyVFS?groupby=Version) [![GitHub license](https://img.shields.io/badge/license-Apache%202-blue.svg)](https://raw.githubusercontent.com/hueifeng/TinyVFS/master/LICENSE)

`TinyVFS` 是一个虚拟文件系统，受ABP vNext框架的启发。它可以将js、css、image、cshtml等文件嵌入到程序集中，
并在运行时可以将它们像物理文件一样去使用。

#### 特点

* 在单体应用中，它可以将前端和后台（管理系统）分到单独项目工程中
* 在开发中它可以让开发人员同时进行开发不同的业务或者模块
* 它可以让我们将系统功能模块拆分后组装到一起

## 快速入门

1、通过Nuget安装组件

```
Install-Package TinyVFS
```

2、注册嵌入文件

编辑web资源项目`.csproj`
```
<ItemGroup>
  <EmbeddedResource Include="MyResources\**\*.*" />
  <Content Remove="MyResources\**\*.*" />
</ItemGroup>
```

通过如下代码片段将文件嵌入到虚拟文件系统。

```csharp
     services.AddVirtualFilesService();
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<WebApplication1.Pages.IndexModel>("WebResources");
            });
```



3、获取虚拟文件

嵌入到程序集后可通过`IVirtualFileProvider`来获取文件或者目录内容

```csharp
public class MyService
{
    private readonly IVirtualFileProvider _virtualFileProvider;

    public MyService(IVirtualFileProvider virtualFileProvider)
    {
        _virtualFileProvider = virtualFileProvider;
    }

    public void Foo()
    {
        //Getting a single file
        var file = _virtualFileProvider.GetFileInfo("/MyResources/js/test.js");
        var fileContent = file.ReadAsString(); //ReadAsString is an extension method of ABP

        //Getting all files/directories under a directory
        var directoryContents = _virtualFileProvider.GetDirectoryContents("/MyResources/js");
    }
}
```

4、动态监听文件

当我们在本机进行开发时，也许我们会对资源项目中的静态文件进行修改，那么常规操作可以能我们去重新生成代码.....

现在我们可以通过`ReplaceEmbeddedByPhysical `来通过浏览器刷新即可获取最新的文件信息

```csharp
  services.AddVirtualFilesService();
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<WebApplication1.Pages.IndexModel>(
                 Path.Combine(WebHostEnvironment.ContentRootPath, "..\\WebResources")
             );
            });

```

5、虚拟文件中间件

虚拟文件中间件用于向客户端/浏览器提供嵌入式(js, css, image ...)文件, 
就像 wwwroot 文件夹中的物理(静态)文件一样. 在静态文件中间件之后添加它, 如下所示:

```csharp
app.UseVirtualFiles();
```

如果想扩展其他文件格式那么，可使用重载方法，如下所示:

```csharp
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".less"] = "text/css";
app.UseVirtualFiles(provider);
```


通过设置虚拟文件中间件，使在虚拟文件相同的位置放置物理文件，从而使物理文件覆盖虚拟文件成为可能。

6、ASP.NET Core集成

虚拟文件可以直接集成到ASP.NET Core中

· 虚拟文件可以像Web应用程序中的物理静态文件一样使用。
· Razor Views, Razor Pages, js, css, 图像文件和所有其他Web内容可以嵌入到程序集中并像物理文件一样使用。
· 应用程序可以覆盖模块（web资源）的虚拟文件, 就像将具有相同名称和扩展名的文件放入虚拟文件的同一文件夹中一样.

7、Views & Pages

它们不需要任何配置，可在我们应用程序中使用，当我们物理目录存在这些文件时，则覆盖虚拟文件。


## 贡献

如果您有想法可以加入进来，或者发现本项目中有需要改进的代码，欢迎Fork并提交PR！


## 参考

- [ABP vNext](https://github.com/abpframework/abp)
