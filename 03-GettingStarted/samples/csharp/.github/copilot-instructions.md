# MCP Calculator Server - AI Instructions

This repository implements a Model Context Protocol (MCP) server for basic calculator operations using .NET 8.

## Architecture & Core Components

- **MCP Server Implementation**:
  - `src/McpCalculatorServer.cs`: Contains the core logic. Tools are defined as static methods in the `Calculator.Tools.McpCalculatorServer` class.
  - **Attributes**: Uses `[McpServerTool]` to mark methods as tools and `[Description]` to provide documentation for the LLM.
  - **Transport**: Uses `stdio` transport for communication.

- **Server Configuration**:
  - `src/McpCalculatorExtensions.cs`: Handles dependency injection and server startup.
  - Uses `AddMcpServer()`, `WithStdioServerTransport()`, and `WithToolsFromAssembly()` to register tools automatically.
  - **Logging**: Configured to output to `StandardError` to avoid interfering with the `stdio` protocol on `StandardOutput`.

## Project Structure

- `src/`: Core library project (`calculator.csproj`). Generates the `McpCalculatorTools` NuGet package.
- `examples/console-app/`: A sample console application that consumes the library.

## Developer Workflows

### Building and Running
- **Build**: `dotnet build`
- **Run**: `dotnet run --project src/calculator.csproj`
- **Docker**:
  - Build: `docker build -t mcp-calculator src/`
  - Run: `docker run --rm -i mcp-calculator`

### Testing with MCP Clients
- Configure `.vscode/mcp.json` to test with VS Code or Claude Desktop.
- The server communicates via `stdio`, so it must be launched by the client.

## Coding Conventions

- **Tool Definition**:
  - Create public static methods in `McpCalculatorServer`.
  - Decorate with `[McpServerTool]` and `[Description("...")]`.
  - Parameter names and types are automatically inferred by the SDK.
  
  ```csharp
  [McpServerTool, Description("Calculates the sum of two numbers")]
  public static double Add(double numberA, double numberB)
  {
      return numberA + numberB;
  }
  ```

- **Logging**:
  - Always log to `stderr`.
  - Use `builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace)` in the host setup.

## Dependencies
- `ModelContextProtocol`: The core MCP SDK for .NET.
- `Microsoft.Extensions.Hosting`: For the generic host and dependency injection.
