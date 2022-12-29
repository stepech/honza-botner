using System.Threading.Tasks;
using Discord;

namespace HonzaBotner.Discord.Managers;

public interface IButtonManager
{
    /// <summary>
    /// Sets up default buttons on verification messages
    /// </summary>
    /// <param name="target">Target message where the buttons will be added</param>
    Task SetupVerificationButtons(IUserMessage target);

    /// <summary>
    /// Removes all button interactions from provided message
    /// </summary>
    /// <param name="target">Target message</param>
    /// <returns></returns>
    Task RemoveButtonsFromMessage(IUserMessage target);
}
