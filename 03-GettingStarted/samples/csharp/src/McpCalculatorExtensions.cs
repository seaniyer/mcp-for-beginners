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