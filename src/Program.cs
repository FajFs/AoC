var builder = Host.CreateApplicationBuilder(args);
builder.AddApplicationDefaults();
var app = builder.Build(); 

var day = app.Services.GetRequiredService<DayTwo>();
await day.SolveAsync();



