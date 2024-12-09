var builder = Host.CreateApplicationBuilder(args);
builder.AddApplicationDefaults();
var app = builder.Build(); 

await app.ResolveAocDay().SolveAsync();