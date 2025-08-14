$content = @"
# MarsCalcMCP

A sample MCP (Model Context Protocol) server implemented in C#/.NET 9. It exposes simple calculator tools over stdio using the MCP C# SDK.

## Features
- Tools: Add, Subtract, Multiply, Divide, IsPrime
- Stdio transport via Microsoft.Extensions.Hosting
- Packaged as an MCP server NuGet package
- Targets .NET 9

## Prerequisites
- .NET SDK 9.0 or later

## Build and Run (local)
```bash
# From the src directory
dotnet build

# Run the MCP server over stdio
dotnet run
Using from an MCP client
This project includes an MCP server manifest at src/.mcp/server.json. Many MCP-compatible clients can read this file to discover and install the server from NuGet.
Minimal server manifest example:
{
  "$schema": "https://modelcontextprotocol.io/schemas/draft/2025-07-09/server.json",
  "name": "io.github.seaniyer/mcp-for-beginners",
  "description": "An MCP server using the MCP C# SDK.",
  "packages": [
    {
      "registry_name": "nuget",
      "name": "MarsCalcMCP",
      "version": "0.1.0-beta"
    }
  ]
}
Exposed tools
•	Add(numberA: double, numberB: double) -> double
•	Subtract(numberA: double, numberB: double) -> double
•	Multiply(numberA: double, numberB: double) -> double
•	Divide(numberA: double, numberB: double) -> double
•	Throws: "Cannot divide by zero" if numberB is 0
•	IsPrime(number: long) -> bool
Project structure
•	Program.cs: Host builder and MCP server wiring
•	McpCalculatorServer.cs: Tool implementations annotated with [McpServerToolType] / [McpServerTool]
•	.mcp/server.json: MCP server manifest for client discovery
•	calculator.csproj: .NET project and NuGet packaging configuration
Packaging
This project is configured to build a NuGet package as an MCP server.
•	Build and produce the package:
dotnet build -c Release
# Package will be emitted under: src/bin/Release/*.nupkg
•	The project enables:
•	<PackAsTool>true</PackAsTool>
•	<PackageType>McpServer</PackageType>
•	<GeneratePackageOnBuild>true</GeneratePackageOnBuild>
•	<PackageReadmeFile>README.md</PackageReadmeFile>
Notes
•	Transport: stdio
•	Target framework: net9.0
License
See the repository for license details. "@ Set-Content -Path "src/README.md" -Value $content -Encoding UTF8