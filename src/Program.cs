var builder = Host.CreateApplicationBuilder(args);
builder.AddApplicationDefaults();
var app = builder.Build(); 

await app.ResolveAocDay(day: 6).SolveAsync();