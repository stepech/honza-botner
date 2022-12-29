using System.Threading.Tasks;
using Discord;

namespace HonzaBotner.Discord.Attributes;

public interface ICustomAttribute
{
    public Task<Embed> BuildFailedCheckDiscordEmbed();
}
