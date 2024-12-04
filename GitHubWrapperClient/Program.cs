using GitHubWrapperServer;
using Grpc.Net.Client;


var serverAddress = "http://localhost:5207";


using var channel = GrpcChannel.ForAddress(serverAddress);


var client = new GithubWrapper.GithubWrapperClient(channel);
Console.Write("Enter Search Query:");
string? searchQuery = Console.ReadLine();
var response = await client.SearchReposAsync(new RepoReq
{
    Query = searchQuery
});


Console.WriteLine($"Total Repositories: {response.TotalCount}");
for (int i = 0; i < response.Repos.Count; i++)
{
    Console.WriteLine($"[{i}] Repo Name: {response.Repos[i].Name}, Owner: {response.Repos[i].OwnerLogin}");
}

