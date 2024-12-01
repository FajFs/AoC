namespace AoC.Extensions;
public static class Extensions
{
    private record AocCookie(string Name, string Value);

    public static IHostApplicationBuilder AddApplicationDefaults(this IHostApplicationBuilder builder)
    {
        // add serilog
        var logger = new LoggerConfiguration()
            //remove microsoft logs
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //remove http client logs
            .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
            .WriteTo.Console()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        // add user secrets
        builder.Configuration.AddUserSecrets<AocCookie>();

        // add http client
        var Uri = new Uri("https://adventofcode.com/");
        var cookie = builder.Configuration.GetSection(nameof(AocCookie)).Get<AocCookie>();

        builder.Services.AddHttpClient<AdventOfCodeClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = Uri;
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(Uri, new Cookie(cookie!.Name, cookie.Value));
                return new HttpClientHandler() { CookieContainer = cookieContainer };
            });

        //register services
        builder.Services.AddSingleton<FileSystemCache>();

        // register all days
        builder.Services.AddRangeTransient<IDay>();

        return builder;
    }

    public static IDay ResolveAocDay(this IHost app, int? year = null, int? day = null)
    {
        var resolvedYear = year ?? DateTime.Now.Year;
        var resolvedDay = day ?? DateTime.Now.Day;

        var fullName = $"AoC.Year{resolvedYear}.Day{resolvedDay:D2}";

        var type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .FirstOrDefault(t => t.FullName == fullName && typeof(IDay).IsAssignableFrom(t)) 
            ?? throw new InvalidOperationException($"Type '{fullName}' not found or does not implement IDay.");

        return (IDay)app.Services.GetRequiredService(type);
    }

    private static IServiceCollection AddRangeTransient<TService>(this IServiceCollection services)
    {
        foreach (var dayService in typeof(TService).Assembly.GetTypes()
            .Where(x => typeof(TService).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract))
                services.AddTransient(dayService);

        return services;
    }
}
