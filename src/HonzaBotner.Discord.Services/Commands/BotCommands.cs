using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HonzaBotner.Discord.Managers;
using HonzaBotner.Discord.Services.Options;
using Microsoft.Extensions.Options;

namespace HonzaBotner.Discord.Services.Commands;

public class BotCommands : InteractionModuleBase<SocketInteractionContext>
{
    private readonly InfoOptions _infoOptions;
    private readonly IGuildProvider _guildProvider;

    public BotCommands(IOptions<InfoOptions> options, IGuildProvider guildProvider)
    {
        _infoOptions = options.Value;
        _guildProvider = guildProvider;
    }

    [SlashCommand("bot", "Get info about bot. Version, source code, etc")]
    public async Task InfoCommandAsync()
    {
        const string content = "This bot is developed by the community.\n" +
                               "We will be happy if you join the development and help us further improve it.";

        SocketGuild guild = _guildProvider.GetCurrentGuild();
        IGuildUser bot = guild.GetUser(Context.Client.CurrentUser.Id);
        EmbedBuilder embed = new()
            {
                Author = new EmbedAuthorBuilder { Name = bot.DisplayName, IconUrl = bot.GetAvatarUrl() },
                Title = "Information about the bot",
                Description = content,
                Color = Color.Blue
            };

        string version = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? "<unknown version>";
        if (!string.IsNullOrEmpty(_infoOptions.VersionSuffix))
        {
            version += "-" + _infoOptions.VersionSuffix;
        }

        embed.AddField("Version:", version);

        var buttons = new ComponentBuilder()
            .WithButton("Source code", url: _infoOptions.RepositoryUrl, emote: new Emoji("📜"))
            .WithButton("Report an issue", url: _infoOptions.IssueTrackerUrl, emote: new Emoji("🐛"))
            .WithButton("News", url: _infoOptions.ChangelogUrl, emote: new Emoji("〽️"));

        await RespondAsync(embed: embed.Build(), components: buttons.Build());
    }

    [SlashCommand("ping", "pong?")]
    public async Task PingCommandAsync(InteractionContext ctx)
    {
        await RespondAsync($"Pong!");
    }

    [Group("buttons", "Commands to manage buttons")]
    public class ButtonCommands : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IButtonManager _buttonManager;

        public ButtonCommands(IButtonManager manager)
        {
            _buttonManager = manager;
        }

        [SlashCommand("remove","Deletes all button interactions on the message")]
        public async Task RemoveButtons(
            [Summary("message-link", "URL  of the message")] string url)
        {
            SocketGuild guild = Context.Guild;
            if (await DiscordHelper.FindMessageFromLink(guild, url) is not IUserMessage message)
            {
                await RespondAsync($"Couldn't find message with link: {url}", ephemeral:true);
                return;
            }

            try
            {
                await _buttonManager.RemoveButtonsFromMessage(message);
            }
            catch (Exception)
            {
                await RespondAsync("Error: You can only edit messages by this bot.", ephemeral: true);
                return;
            }

            await RespondAsync("Removed buttons");
        }

        [SlashCommand("setup","Marks message as verification message")]
        public async Task SetupButtons(
            [Summary("message-link", "URL  of the message")] string url
        )
        {
            SocketGuild guild = Context.Guild;
            if (await DiscordHelper.FindMessageFromLink(guild, url) is not IUserMessage message)
            {
                await RespondAsync($"Couldn't find message with link: {url}", ephemeral: true);
                return;
            }

            try
            {
                await _buttonManager.SetupVerificationButtons(message);
            }
            catch (Exception)
            {
                await RespondAsync("Error: You can only edit messages by this bot.",ephemeral:true);
                return;
            }

            await RespondAsync("Added verification buttons to the message.");
        }
    }
}
