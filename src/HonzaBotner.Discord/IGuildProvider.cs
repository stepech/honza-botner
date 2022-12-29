using Discord;
using Discord.WebSocket;

namespace HonzaBotner.Discord;

public interface IGuildProvider
{
    public ulong GuildId { get; }
    SocketGuild GetCurrentGuild();
}
