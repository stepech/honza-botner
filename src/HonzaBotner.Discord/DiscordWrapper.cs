using System;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HonzaBotner.Discord;

public class DiscordWrapper
{
    public DiscordSocketClient Client { get; }
    public SocketInteraction Interactivity { get; }

    public DiscordWrapper(IOptions<DiscordConfig> options, IServiceProvider services, ILoggerFactory loggerFactory)
    {
        DiscordSocketConfig config = new()
        {
            AlwaysDownloadUsers = true,
            GatewayIntents = GatewayIntents.All
        };

        Client = new DiscordSocketClient(config);

        InteractionServiceConfig sConfig = new()
        {
            DefaultRunMode = RunMode.Async,
            EnableAutocompleteHandlers = false,
        };
        Interactivity = SocketInteraction(Client);

        Client.Logger.LogInformation("Starting Bot");
    }
}
