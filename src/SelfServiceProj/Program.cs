using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>{

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
