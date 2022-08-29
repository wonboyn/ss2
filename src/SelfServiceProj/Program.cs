using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {

        // Add our global configuration instance
        services.AddSingleton(options =>
        {
            var configuration = context.Configuration;
            return configuration;
        });

        // Inject the Newtonsoft JSON library
        services.AddHttpClient().AddControllers().AddNewtonsoftJson();

        // Inject the Bot Framework Authentication
        services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();

        // Inject the Bot Adapter
        services.AddSingleton<IBotFrameworkHttpAdapter, SelfServiceProj.BotAdapter>();

        // Inject the Bot itself
        services.AddTransient<IBot, SelfServiceProj.BotHandler>();
    })
    .Build();

host.Run();
