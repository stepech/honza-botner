using System;
using System.Threading.Tasks;
using Discord;
using HonzaBotner.Discord.Managers;
using HonzaBotner.Services.Contract.Dto;
using Microsoft.Extensions.Logging;

namespace HonzaBotner.Discord.Services.Managers;

public class ReminderManager : IReminderManager
{
    private readonly IGuildProvider _guildProvider;
    private readonly ILogger<ReminderManager> _logger;

    public ReminderManager(IGuildProvider guildProvider, ILogger<ReminderManager> logger)
    {
        _guildProvider = guildProvider;
        _logger = logger;
    }

    public async Task<Embed> CreateDmReminderEmbedAsync(Reminder reminder)
    {
        var guild = _guildProvider.GetCurrentGuild();

        return await _CreateReminderEmbed(
            $"🔔 Reminder from {guild.Name} Discord",
            reminder,
            Color.Gold,
            true
        );
    }

    public async Task<Embed> CreateReminderEmbedAsync(Reminder reminder)
    {
        return await _CreateReminderEmbed(
            "🔔 Reminder",
            reminder,
            Color.Gold,
            true
        );
    }

    public async Task<Embed> CreateExpiredReminderEmbedAsync(Reminder reminder)
    {
        return await _CreateReminderEmbed(
            "🔕 Expired reminder",
            reminder,
            Color.DarkGrey
        );
    }

    public async Task<Embed> CreateCanceledReminderEmbedAsync(Reminder reminder)
    {
        return await _CreateReminderEmbed(
            "🛑 Canceled reminder",
            reminder,
            Color.Red
        );
    }

    private async Task<Embed> _CreateReminderEmbed(
        string title,
        Reminder reminder,
        Color color,
        bool useDateTime = false
    )
    {
        var guild = _guildProvider.GetCurrentGuild();

        IGuildUser? author = guild.GetUser(reminder.OwnerId);

        string datetime = useDateTime
            ? "\n\n<t:" + Math.Floor(reminder.DateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) + ":f>"
            : "";

        var embedBuilder = new EmbedBuilder()
            .WithTitle(title)
            .WithAuthor(
                author?.DisplayName ?? "Unknown user",
                iconUrl: author?.GetAvatarUrl())
            .WithDescription(reminder.Content + datetime)
            .WithColor(color);

        return embedBuilder.Build();
    }
}
