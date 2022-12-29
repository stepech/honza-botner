using System.Threading.Tasks;
using Discord;

namespace HonzaBotner.Discord.Managers;

public interface IVoiceManager
{
    Task AddNewVoiceChannelAsync(
        IVoiceChannel channelToCloneFrom,
        IGuildUser user,
        string? name = null,
        long? limit = null,
        bool? isPublic = null
    );

    Task<bool> EditVoiceChannelAsync(IGuildUser member, string? name, long? limit, bool? isPublic);

    Task DeleteUnusedVoiceChannelAsync(IGuildUser channel);

    Task DeleteAllUnusedVoiceChannelsAsync();
}
