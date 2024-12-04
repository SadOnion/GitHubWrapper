using Grpc.Core;
using RestSharp;
using System.Text.Json;

namespace GitHubWrapperServer.Services
{
    public class GitHubWrapperService : GithubWrapper.GithubWrapperBase
    {
        private readonly string? apiKey;
        private readonly string baseUrl = "https://api.github.com";
        private readonly RestClient restClient;

        public GitHubWrapperService(IConfiguration configuration)
        {
            var githubSection = configuration.GetSection("GitHub");
            apiKey = githubSection.GetValue<string>("ApiKey");
            restClient = new RestClient(baseUrl);
        }

        public override async Task<RepoResp> SearchRepos(RepoReq request, ServerCallContext context)
        {

            var response = await ExecuteSearchRequestAsync(request.Query);


            if (response.IsSuccessful)
            {
                try
                {
                    using var document = JsonDocument.Parse(response.Content ?? string.Empty);
                    return GetRepoRespFromJson(document);
                }
                catch (Exception e)
                {
                    throw new RpcException(new Status(StatusCode.Internal, $"Failed to read json response, {e.Message}"));
                }
            }

            throw new RpcException(new Status(StatusCode.Internal, $"Failed to fetch GitHub repositories, {response.ErrorMessage}"));
        }

        private async Task<RestResponse> ExecuteSearchRequestAsync(string query)
        {
            var request = new RestRequest("search/repositories", Method.Get);

            request.AddQueryParameter("q", query);

            request.AddHeader("Accept", "application/vnd.github+json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            request.AddHeader("X-GitHub-Api-Version", "2022-11-28");


            return await restClient.ExecuteAsync(request);
        }

        private RepoResp GetRepoRespFromJson(JsonDocument jsonDocument)
        {
            var root = jsonDocument.RootElement;

            var repoResponse = new RepoResp()
            {
                TotalCount = root.GetProperty("total_count").GetInt32(),
                Repos = { root.GetProperty("items").EnumerateArray()
                    .Select(item => new Repo
                    {
                        Name = item.GetProperty("name").GetString(),
                        OwnerLogin = item.GetProperty("owner").GetProperty("login").GetString()
                    }) }
            };
            return repoResponse;
        }
    }
}
