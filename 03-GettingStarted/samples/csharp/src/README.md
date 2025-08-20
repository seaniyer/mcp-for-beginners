# MCP Calculator Tools

A NuGet package that provides calculator tools for Model Context Protocol (MCP) servers. This package includes basic arithmetic operations and prime number validation tools that can be easily integrated into any .NET MCP server.

## Features

- **Add**: Calculate the sum of two numbers
- **Subtract**: Calculate the difference of two numbers
- **Multiply**: Calculate the product of two numbers
- **Divide**: Calculate the quotient of two numbers (with zero-division protection)
- **IsPrime**: Validate if a number is prime using the 6k±1 optimization

## Installation

```bash
dotnet add package McpCalculatorTools
```

## Quick Start

### Using as a Console Application

```csharp
using McpCalculatorTools;

// Run the MCP Calculator Server
await McpCalculatorExtensions.RunMcpCalculatorServerAsync(args);
```

### Using in an Existing Host Application

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using McpCalculatorTools;

var builder = Host.CreateApplicationBuilder(args);

// Add MCP Calculator Tools to your existing host
builder.Services.AddMcpCalculatorTools();

// Add other services...
// builder.Services.AddOtherServices();

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

## Requirements

- .NET 8.0 or later
- ModelContextProtocol package

## License

MIT License - see LICENSE file for details.