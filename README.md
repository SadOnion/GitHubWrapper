
# GitHub Wrapper Service

This project implements a gRPC service that wraps GitHub's REST API to search for repositories. It uses `RestSharp` to interact with GitHub's API and `gRPC` for communication between clients and the server.

## Features

- **gRPC Service**: Exposes a `SearchRepos` method to search GitHub repositories.
- **GitHub API Integration**: Utilizes GitHub's REST API to search for repositories by query.
- **C# Implementation**: Uses `Grpc.Net.Client` for gRPC communication and `RestSharp` to make HTTP requests to GitHub's API.

## Setup and Installation

### Prerequisites

- [.NET 6.0 or later](https://dotnet.microsoft.com/download/dotnet)
- A GitHub Personal Access Token (PAT) to authenticate API requests.

### Clone the repository

```bash
git clone https://github.com/sadonion/github-wrapper-service.git
cd github-wrapper-service
```

### Configure GitHub API Token

1. Create a `appsettings.json` file in the root of the project if it doesn't exist.
2. Add your GitHub API token:

```json
{
  "GitHub": {
    "ApiKey": "your-github-api-token"
  }
}
```

Replace `"your-github-api-token"` with your actual GitHub Personal Access Token.

### Build the project

Use the following commands to build the project:

```bash
dotnet build
```

### Run the gRPC server

To start the server, run the following command:

```bash
dotnet run
```

By default, the gRPC server will run on `https://localhost:5001`. You can change this in the `appsettings.json` file or in the server code.

## Using the gRPC Service

To use the service, create a client that connects to the server and invokes the `SearchRepos` method. You can test the service using the following client code:

### Client Example

```csharp
using Grpc.Net.Client;
using GitHubWrapperServer;  // Your namespace here
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var serverAddress = "https://localhost:5001"; // Address of the running gRPC server

        using var channel = GrpcChannel.ForAddress(serverAddress);
        var client = new GithubWrapperClient(channel);

        var response = await client.SearchReposAsync(new RepoReq
        {
            Query = "dotnet" // Query for searching repositories on GitHub
        });

        Console.WriteLine($"Total Repositories: {response.TotalCount}");

        foreach (var repo in response.Respos)
        {
            Console.WriteLine($"Repo Name: {repo.Name}, Owner: {repo.OwnerLogin}");
        }
    }
}
```

Make sure to update the `serverAddress` with the correct URL for your gRPC server.

## Contributing

Feel free to fork the repository, create a branch, and submit pull requests. If you have any ideas or find any issues, please create an issue on GitHub.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
