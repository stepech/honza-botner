﻿using DSharpPlus.Entities;

namespace HonzaBotner.Discord.Attributes;

public interface IOnlyForModsAttribute
{
    DiscordEmbed GetFailedCheckDiscordEmbed()
    {
        return new DiscordEmbedBuilder()
            .WithTitle("Přístup zakázán")
            .WithDescription("Tento příkaz může používat pouze Moderátor.")
            .WithColor(DiscordColor.Violet)
            .Build();
    }
}
