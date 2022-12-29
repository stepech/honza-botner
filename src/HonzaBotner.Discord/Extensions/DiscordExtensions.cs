using System;
using System.Threading.Tasks;
using Discord;

namespace HonzaBotner.Discord.Extensions;

public static class DiscordExtensions
{
    public static async Task ReportException(this ITextChannel channel, string source, Exception exception)
    {
        static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        await channel.SendMessageAsync(
            embed: new EmbedBuilder()
                .WithTitle($"{source} - {exception.GetType().Name}")
                .WithColor(Color.Red)
                .AddField("Message:", exception.Message, true)
                .AddField("Stack Trace:", Truncate(exception.StackTrace ?? "No stack trace", 500))
                .WithTimestamp(DateTime.UtcNow)
                .WithDescription(
                    "Please react to this message to indicate that it is already logged in isssue or solved"
                )
                .Build()
        );
    }
}
