# NuGet Package Build and Publish Workflow

This GitHub Actions workflow automatically builds and publishes the McpCalculatorTools NuGet package from the C# project located at `03-GettingStarted/samples/csharp/src/calculator.csproj`.

## Workflow Triggers

- **Push to main**: Triggers when changes are pushed to the main branch in the C# project path
- **Pull Request**: Runs build and test steps for pull requests to validate changes
- **Release**: Publishes to NuGet.org when a GitHub release is published
- **Manual**: Can be triggered manually via workflow_dispatch

## Jobs Overview

### 1. Build and Test (`build-and-test`)
- Restores dependencies
- Builds the project in Release configuration
- Runs tests (if any exist)
- Creates NuGet package
- Uploads package as build artifact

### 2. Publish to GitHub Packages (`publish-github`)
- Runs only on push to main branch
- Downloads build artifacts
- Publishes package to GitHub Packages
- Accessible to repository collaborators

### 3. Publish to NuGet.org (`publish-nuget`)
- Runs only on GitHub releases
- Downloads build artifacts
- Publishes package to public NuGet.org using OIDC authentication (no API key required)

## Setup Requirements

### For NuGet.org Publishing

To publish to NuGet.org using OIDC tokens, you need to set up trusted publishing:

1. **Sign in to NuGet.org** and navigate to your account settings
2. **Go to "Trusted publishers"** at https://www.nuget.org/account/trustedpublishing
3. **Add a trusted publisher** with the following details:
   - **Repository**: `seaniyer/mcp-for-beginners`
   - **Workflow**: `.github/workflows/nuget-publish.yml`
   - **Package**: `McpCalculatorTools`
4. **Create a GitHub release** to trigger the publishing workflow

This eliminates the need for storing API keys as secrets and provides more secure authentication using GitHub's OIDC tokens.

### For GitHub Packages

No additional setup is required - the workflow uses the built-in `GITHUB_TOKEN` for authentication.

## Package Information

- **Package ID**: McpCalculatorTools
- **Version**: 1.0.0 (configured in calculator.csproj)
- **Target Framework**: .NET 8.0
- **Dependencies**: Microsoft.Extensions.Hosting, ModelContextProtocol

## Usage

Once published, the package can be installed using:

```bash
# From NuGet.org (after release)
dotnet add package McpCalculatorTools

# From GitHub Packages
dotnet add package McpCalculatorTools --source https://nuget.pkg.github.com/seaniyer/index.json
```