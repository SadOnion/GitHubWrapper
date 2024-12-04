using GitHubWrapperServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GitHubWrapperService>();

app.Run();
