using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace HonzaBotner.Discord;

public static class DiscordHelper
{
    /// <summary>
    /// Finds DiscordMessage from Discord link to message.
    /// </summary>
    /// <param name="guild">Discord guild to find the message at.</param>
    /// <param name="link">URL of the message.</param>
    /// <returns>DiscordMessage if successful, otherwise null.</returns>
    public static async Task<IMessage?> FindMessageFromLink(SocketGuild guild, string link)
    {
        // Match the channel and message IDs.
        const string pattern = @"https://discord(?:app)?\.com/channels/(?:\d+)/(\d+)/(\d+)/?";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(link);

        // Malformed message link.
        if (!match.Success) return null;

        try
        {
            bool channelParseSuccess = ulong.TryParse(match.Groups[1].Value, out ulong channelId);
            if (!channelParseSuccess) return null;

            ITextChannel? channel = guild.GetChannel(channelId) as ITextChannel;

            if (channel is null) return null;

            bool messageParseSuccess = ulong.TryParse(match.Groups[2].Value, out ulong messageId);
            if (!messageParseSuccess) return null;

            return await channel.GetMessageAsync(messageId);
        }
        catch
        {
            return null;
        }
    }
}
