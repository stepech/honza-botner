﻿using System;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace HonzaBotner.Discord;

public class ConfigGuildProvider : IGuildProvider
{
    public ulong GuildId { get; }
    private readonly DiscordSocketClient _client;

    public ConfigGuildProvider(DiscordWrapper wrapper, IOptions<DiscordConfig> config)
    {
        if (config.Value.GuildId is null)
        {
            throw new InvalidOperationException("GuildId not configured");
        }
        GuildId = config.Value.GuildId ?? 0;
        _client = wrapper.Client;
    }

    public SocketGuild GetCurrentGuild()
    {
        return _client.GetGuild(GuildId);
    }
}
