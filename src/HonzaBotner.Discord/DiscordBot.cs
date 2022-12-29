using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using HonzaBotner.Discord.Extensions;
using HonzaBotner.Discord.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HonzaBotner.Discord;

internal class DiscordBot : IDiscordBot
{
    private readonly DiscordWrapper _discordWrapper;
    private readonly EventHandler.EventHandler _eventHandler;
    private readonly CommandsConfigurator _commandsConfigurator;
    private readonly IVoiceManager _voiceManager;
    private readonly DiscordConfig _discordOptions;

    private DiscordSocketClient Client => _discordWrapper.Client;
    private SlashCommandsExtension Commands => _discordWrapper.Commands;

    private readonly ILogger<DiscordBot> _logger;

    public DiscordBot(DiscordWrapper discordWrapper,
        EventHandler.EventHandler eventHandler,
        CommandsConfigurator commandsConfigurator,
        IVoiceManager voiceManager,
        IOptions<DiscordConfig> discordOptions,
        ILogger<DiscordBot> logger)
    {
        _discordWrapper = discordWrapper;
        _eventHandler = eventHandler;
        _commandsConfigurator = commandsConfigurator;
        _voiceManager = voiceManager;
        _discordOptions = discordOptions.Value;
        _logger = logger;
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        Client.LoggedIn += Client_Ready;
        Client.GuildAvailable += Client_GuildAvailable;
        Client.ClientErrored += Client_ClientError;
        Client.GuildDownloadCompleted += Client_GuildDownloadCompleted;

        Commands.SlashCommandInvoked += Commands_CommandInvoked;
        Commands.SlashCommandErrored += Commands_CommandErrored;
        Commands.ContextMenuErrored += Commands_ContextMenuErrored;
        Commands.AutocompleteErrored += Commands_AutocompleteErrored;

        Client.ComponentInteractionCreated += Client_ComponentInteractionCreated;
        Client.ReactionAdded += Client_MessageReactionAdded;
        Client.ReactionRemoved += Client_MessageReactionRemoved;
        Client.UserVoiceStateUpdated += Client_VoiceStateUpdated;
        Client.GuildMemberUpdated += Client_GuildMemberUpdated;
        Client.ChannelCreated += Client_ChannelCreated;
        Client.ThreadCreated += Client_ThreadCreated;

        _commandsConfigurator.Config(Commands);

        await Client.ConnectAsync();
        await Task.Delay(-1, cancellationToken);
    }

    private Task Client_Ready()
    {
        _logger.LogInformation("Client is ready to process events");
        return Task.CompletedTask;
    }

    private Task Client_GuildAvailable(SocketGuild guild)
    {
        _logger.LogInformation("Guild available: {GuildName}", guild.Name);
        return Task.CompletedTask;
    }

    private async Task Client_GuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs e)
    {
        sender.Logger.LogInformation("Guild download completed");

        // Run managers' init processes.
        await _voiceManager.DeleteAllUnusedVoiceChannelsAsync();
    }

    private async Task Client_ClientError(DiscordClient sender, ClientErrorEventArgs e)
    {
        sender.Logger.LogError(e.Exception, "Exception occured");
        await ReportException( "Client error", e.Exception);
        e.Handled = true;
    }

    private Task Commands_CommandInvoked(SlashCommandsExtension e, SlashCommandInvokedEventArgs args)
    {
        e.Client.Logger.LogDebug("Received {Command} by {Author}", args.Context.CommandName, args.Context.User.Username);
        return Task.CompletedTask;
    }

    private async Task Commands_CommandErrored(SlashCommandsExtension e, SlashCommandErrorEventArgs args)
    {
        e.Client.Logger.LogError(args.Exception, "Exception occured while executing {Command}", args.Context.CommandName);
        await ReportException($"SlashCommand {args.Context.CommandName}", args.Exception);
        args.Handled = true;
        await args.Context.Channel.SendMessageAsync("Something failed");
    }

    private async Task Commands_ContextMenuErrored(SlashCommandsExtension e, ContextMenuErrorEventArgs args)
    {
        e.Client.Logger.LogError(args.Exception, "Exception occured while executing context menu {ContextMenu}", args.Context.CommandName);
        await ReportException($"ContextMenu {args.Context.CommandName}", args.Exception);
        args.Handled = true;
        await args.Context.Channel.SendMessageAsync("Something failed");
    }

    private async Task Commands_AutocompleteErrored(SlashCommandsExtension e, AutocompleteErrorEventArgs args)
    {
        e.Client.Logger.LogError(args.Exception, "Autocomplete failed while looking into option {OptionName}", args.Context.FocusedOption.Name);
        await ReportException($"Command Autocomplete for option {args.Context.FocusedOption.Name}",
            args.Exception);
        args.Handled = true;
    }

    private Task Client_ComponentInteractionCreated(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        return _eventHandler.Handle(args);
    }

    private Task Client_MessageReactionAdded(DiscordClient client, MessageReactionAddEventArgs args)
    {
        return _eventHandler.Handle(args);
    }

    private Task Client_MessageReactionRemoved(DiscordClient client, MessageReactionRemoveEventArgs args)
    {
        return _eventHandler.Handle(args);
    }

    private Task Client_VoiceStateUpdated(DiscordClient client, VoiceStateUpdateEventArgs args)
    {
        return _eventHandler.Handle(args);
    }

    private Task Client_GuildMemberUpdated(DiscordClient client, GuildMemberUpdateEventArgs args)
    {
        return _eventHandler.Handle(args);
    }

    private Task Client_ChannelCreated(DiscordClient client, ChannelCreateEventArgs args)
    {
        return _eventHandler.Handle(args);
    }

    private Task Client_ThreadCreated(DiscordClient client, ThreadCreateEventArgs args)
    {
        return _eventHandler.Handle(args);
    }

    private async Task ReportException(string source, Exception exception)
    {
        ulong logChannelId = _discordOptions.LogChannelId;

        if (logChannelId == default)
        {
            return;
        }

        DiscordChannel channel = await _discordWrapper.Client.GetChannelAsync(logChannelId);

        await channel.ReportException(source, exception);
    }
}
