# How to Update an Existing .NET MCP Project to Produce a NuGet Package

Converting your .NET Model Context Protocol (MCP) server from a standalone console application to a reusable NuGet package opens up new possibilities for code sharing and distribution. This guide walks you through the step-by-step process of transforming a basic MCP server into a packageable library that others can easily consume.

## Why Create a NuGet Package from Your MCP Server?

Creating a NuGet package from your MCP server provides several benefits:

- **Reusability**: Other developers can easily integrate your MCP tools into their projects
- **Distribution**: Share your MCP tools through the official NuGet repository or private feeds
- **Versioning**: Maintain proper version control and dependency management
- **Modularity**: Allow consumers to pick and choose which tools they need
- **Professional deployment**: Follow .NET ecosystem best practices

## Before We Start

This tutorial assumes you have an existing .NET MCP server project. We'll use a calculator MCP server as our example, but the same principles apply to any MCP server project.

### Original Project Structure

```
calculator/
├── calculator.csproj
├── Program.cs
└── McpCalculatorServer.cs
```

### Target Project Structure

```
calculator/
├── src/
│   ├── calculator.csproj (library)
│   ├── McpCalculatorServer.cs
│   ├── McpCalculatorExtensions.cs
│   └── README.md
└── examples/
    └── console-app/
        ├── ConsoleApp.csproj
        └── Program.cs
```

## Step 1: Convert Console Application to Class Library

The first step is to modify your `.csproj` file to target library compilation instead of executable compilation.

### Original .csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="8.*" />
    <PackageReference Include="ModelContextProtocol" Version="0.*-*" />
  </ItemGroup>
</Project>
```

### Updated .csproj with NuGet Package Metadata

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- Remove OutputType=Exe to make this a library -->
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    
    <!-- NuGet Package Properties -->
    <PackageId>McpCalculatorTools</PackageId>
    <Version>1.0.0</Version>
    <Authors>MCP Community</Authors>
    <Company>MCP for Beginners</Company>
    <Description>A collection of calculator tools for Model Context Protocol (MCP) servers. Provides basic arithmetic operations (add, subtract, multiply, divide) and prime number validation as MCP tools.</Description>
    <PackageTags>mcp;model-context-protocol;calculator;arithmetic;tools</PackageTags>
    <PackageProjectUrl>https://github.com/seaniyer/mcp-for-beginners</PackageProjectUrl>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <RepositoryUrl>https://github.com/seaniyer/mcp-for-beginners</RepositoryUrl>
    <RepositoryType>git</RepositoryType>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="8.*" />
    <PackageReference Include="ModelContextProtocol" Version="0.*-*" />
  </ItemGroup>

  <ItemGroup>
    <None Include="README.md" Pack="true" PackagePath="" />
  </ItemGroup>
</Project>
```

### Key Changes Explained

1. **Removed `<OutputType>Exe</OutputType>`**: This changes the project from a console app to a library
2. **Added Package Metadata**: Essential properties for a proper NuGet package
3. **Set `GeneratePackageOnBuild=true`**: Automatically creates the .nupkg file during build
4. **Included README.md**: Provides documentation within the package

## Step 2: Create Extension Methods for Easy Integration

Since your project is now a library, you need to provide a clean API for consumers to integrate your MCP tools. Create an extension class that encapsulates the MCP server setup.

### Create `McpCalculatorExtensions.cs`

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Calculator.Tools;

namespace McpCalculatorTools;

/// <summary>
/// Extension methods for configuring MCP Calculator Tools in a host application.
/// </summary>
public static class McpCalculatorExtensions
{
    /// <summary>
    /// Adds MCP Calculator Tools to the service collection and configures the MCP server.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddMcpCalculatorTools(this IServiceCollection services)
    {
        services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();
        
        return services;
    }

    /// <summary>
    /// Creates and runs an MCP server with calculator tools using the default host configuration.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>A task representing the running server.</returns>
    public static async Task RunMcpCalculatorServerAsync(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        
        // Configure logging to stderr (required for MCP servers)
        builder.Logging.AddConsole(consoleLogOptions =>
        {
            consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
        });
        
        builder.Services.AddMcpCalculatorTools();
        await builder.Build().RunAsync();
    }
}
```

This extension class provides two integration patterns:
- **`AddMcpCalculatorTools`**: For integrating with existing host applications
- **`RunMcpCalculatorServerAsync`**: For creating a standalone MCP server

## Step 3: Remove Original Program.cs

Since your project is now a library, remove the original `Program.cs` file that contained the console application entry point.

## Step 4: Create Package Documentation

Add a `README.md` file to your project that documents how to use the package:

```markdown
# MCP Calculator Tools

A NuGet package that provides calculator tools for Model Context Protocol (MCP) servers.

## Installation

```bash
dotnet add package McpCalculatorTools
```

## Quick Start

### Using as a Console Application

```csharp
using McpCalculatorTools;

await McpCalculatorExtensions.RunMcpCalculatorServerAsync(args);
```

### Using in an Existing Host Application

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using McpCalculatorTools;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMcpCalculatorTools();
await builder.Build().RunAsync();
```

## Available Tools

| Tool Name | Description | Parameters |
|-----------|-------------|------------|
| `Add` | Calculates the sum of two numbers | `numberA` (double), `numberB` (double) |
| `Subtract` | Calculates the difference of two numbers | `numberA` (double), `numberB` (double) |
| `Multiply` | Calculates the product of two numbers | `numberA` (double), `numberB` (double) |
| `Divide` | Calculates the quotient of two numbers | `numberA` (double), `numberB` (double) |
| `IsPrime` | Validates if a number is prime | `number` (long) |
```

## Step 5: Create Example Console Application

To demonstrate how consumers can use your package, create an example console application:

### Create `examples/console-app/ConsoleApp.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="../../src/calculator.csproj" />
  </ItemGroup>
</Project>
```

### Create `examples/console-app/Program.cs`

```csharp
using McpCalculatorTools;

// Run the MCP Calculator Server
await McpCalculatorExtensions.RunMcpCalculatorServerAsync(args);
```

## Step 6: Build and Test Your Package

Build your library project to generate the NuGet package:

```bash
cd src
dotnet build
```

This will create a `.nupkg` file in the `bin/Debug` folder. The package contains:
- Your compiled library
- Dependencies metadata
- README documentation
- All necessary package metadata

Test the example console application:

```bash
cd examples/console-app
dotnet build
dotnet run
```

## Step 7: Publishing Your Package

### Local Testing

Before publishing to NuGet.org, test your package locally:

1. Create a local NuGet source:
   ```bash
   mkdir C:\\local-nuget  # Windows
   mkdir ~/local-nuget    # Linux/macOS
   ```

2. Copy your .nupkg file to the local source

3. Add the local source:
   ```bash
   dotnet nuget add source C:\\local-nuget -n local  # Windows
   dotnet nuget add source ~/local-nuget -n local    # Linux/macOS
   ```

4. Test installation in a new project:
   ```bash
   dotnet new console -n TestApp
   cd TestApp
   dotnet add package McpCalculatorTools --source local
   ```

### Publishing to NuGet.org

When ready to publish publicly:

1. Create an account on [nuget.org](https://www.nuget.org)
2. Generate an API key from your account dashboard
3. Publish your package:
   ```bash
   dotnet nuget push bin/Release/McpCalculatorTools.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
   ```

## Best Practices for MCP NuGet Packages

### 1. Namespace Organization
Use a clear, hierarchical namespace structure:
```csharp
namespace CompanyName.Mcp.DomainArea.Tools
{
    // Your MCP tools here
}
```

### 2. Proper Versioning
Follow [Semantic Versioning](https://semver.org/):
- **Major**: Breaking changes
- **Minor**: New features, backward compatible
- **Patch**: Bug fixes, backward compatible

### 3. Comprehensive Documentation
Include:
- Clear descriptions for all tools
- Parameter documentation
- Usage examples
- Configuration options

### 4. Dependency Management
- Pin major versions for stability
- Use minimal dependencies
- Document dependency requirements

### 5. Multiple Integration Patterns
Provide both:
- Simple "run server" methods for basic use cases
- Flexible extension methods for complex scenarios

## Troubleshooting Common Issues

### Package Build Warnings

**Warning NU5104**: If you see warnings about prerelease dependencies (like `ModelContextProtocol`), consider:
- Using preview package versions (e.g., `1.0.0-preview.1`)
- Documenting the preview nature in your package description

### Assembly Loading Issues

If consumers report assembly loading problems:
- Ensure your package targets appropriate .NET versions
- Test with both project references and package references
- Check for dependency version conflicts

### MCP Server Configuration

Common configuration issues:
- Logging must go to stderr for MCP servers
- Ensure proper host lifetime management
- Test with actual MCP clients like Claude Desktop

## Conclusion

Converting your .NET MCP server to a NuGet package significantly increases its accessibility and reusability. By following this guide, you've:

1. ✅ Converted a console application to a class library
2. ✅ Added proper NuGet package metadata
3. ✅ Created clean integration APIs
4. ✅ Provided comprehensive documentation
5. ✅ Created usage examples

Your MCP tools are now ready to be shared with the broader .NET and MCP community. Remember to maintain good versioning practices, keep documentation updated, and respond to community feedback to build a successful package.

## Next Steps

- **Add Unit Tests**: Consider adding tests for your MCP tools
- **CI/CD Pipeline**: Set up automated builds and publishing
- **Multi-targeting**: Support multiple .NET versions if needed
- **Advanced Features**: Add configuration options, custom transports, or additional tool categories

Happy packaging! 🚀