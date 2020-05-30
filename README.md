# TinyVFS 

[![nuget](https://img.shields.io/nuget/v/TinyVFS.svg?style=flat-square)](https://www.nuget.org/packages/TinyVFS) 
![.NET Core](https://github.com/hueifeng/TinyVFS/workflows/.NET%20Core/badge.svg)
[![stats](https://img.shields.io/nuget/dt/TinyVFS.svg?style=flat-square)](https://www.nuget.org/stats/packages/TinyVFS?groupby=Version) [![GitHub license](https://img.shields.io/badge/license-Apache%202-blue.svg)](https://raw.githubusercontent.com/hueifeng/TinyVFS/master/LICENSE)


Language: English | [中文](README.zh-cn.md)

`TinyVFS` is a virtual file system, inspired by the ABP vNext framework. It can embed js, css, image, cshtml and other files into the assembly,
And they can be used like physical files at runtime.

#### Features

* In a single application, it can divide the front end and back end (management system) into separate project projects.
* In development it allows developers to develop different businesses or modules at the same time.
* It allows us to split the system function modules and assemble them together.

## Quick Start

1、Install Package

```
Install-Package TinyVFS
```

2、Registering Embedded Files

Edit web resource project `.csproj`
```
<ItemGroup>
  <EmbeddedResource Include="MyResources\**\*.*" />
  <Content Remove="MyResources\**\*.*" />
</ItemGroup>
```

Embed the file into the virtual file system with the following code snippet.

```csharp
     services.AddVirtualFilesService();
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<WebApplication1.Pages.IndexModel>("WebResources");
            });
```



3、Getting Virtual Files: IVirtualFileProvider

After embedding into the assembly, you can obtain the file or directory content through `IVirtualFileProvider`

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

4、Dynamic listening file

When we are developing on the local machine, maybe we will modify the static files in the resource project, so the normal operation can let us regenerate the code

Now we can use `ReplaceEmbeddedByPhysical` to refresh through the browser to get the latest file information

```csharp
  services.AddVirtualFilesService();
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<WebApplication1.Pages.IndexModel>(
                 Path.Combine(WebHostEnvironment.ContentRootPath, "..\\WebResources")
             );
            });

```

5、Virtual Files Middleware


Virtual file middleware is used to provide embedded (js, css, image ...) files to the client/browser,
Just like the physical (static) file in the wwwroot folder. Add it after the static file middleware, as shown below:

```csharp
app.UseVirtualFiles();
```

If you want to extend other file formats, you can use the overloaded method, as shown below:

```csharp
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".less"] = "text/css";
app.UseVirtualFiles(provider);
```


By setting the virtual file middleware, it is possible to place the physical file in the same position of the virtual file, thereby making it possible for the physical file to cover the virtual file.

6、ASP.NET Core Integration


Virtual files can be integrated directly into ASP.NET Core


-  Virtual files can be used like physical static files in web applications.
-  Razor Views, Razor Pages, js, css, image files and all other web content can be embedded in the assembly and used like physical files.
-  The application can overwrite the virtual file of the module (web resource), just like putting a file with the same name and extension into the same folder of the virtual file.

7、Views & Pages


They do not require any configuration and can be used in our applications. When these files exist in our physical directory, they overwrite the virtual files.

## Contribution

If you have any ideas you can join in, or find that there is code in this project that needs improvement, welcome to Fork and submit a PR!


## Reference

- [ABP vNext](https://github.com/abpframework/abp)
