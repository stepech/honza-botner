using System.Linq;
using System.Threading.Tasks;
using Discord;
using HonzaBotner.Discord.Managers;
using HonzaBotner.Discord.Services.Options;
using HonzaBotner.Discord.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HonzaBotner.Discord.Services.Managers;

public class ButtonManager : IButtonManager
{
    private readonly ButtonOptions _buttonOptions;
    private readonly ILogger<ButtonManager> _logger;
    private readonly ITranslation _translation;

    public ButtonManager(
        IOptions<ButtonOptions> buttonConfig,
        ILogger<ButtonManager> logger,
        ITranslation translation)
    {
        _buttonOptions = buttonConfig.Value;
        _logger = logger;
        _translation = translation;
    }

    public async Task SetupVerificationButtons(IUserMessage message)
    {
        if (_buttonOptions.VerificationId is null || _buttonOptions.StaffVerificationId is null)
        {
            _logger.LogWarning("'VerificationId' or 'StaffVerificationId' not set in config");
            return;
        }

        if (_buttonOptions.CzechChannelsIds?.Contains(message.Channel.Id) ?? false)
        {
            _translation.SetLanguage(ITranslation.Language.Czech);
        }

        var builder = new ComponentBuilder()
            .WithButton(
                _translation["VerifyRolesButton"],
                _buttonOptions.VerificationId,
                ButtonStyle.Primary,
                new Emoji("✅")
            )
            .WithButton(
                _translation["VerifyStaffRolesButton"],
                _buttonOptions.StaffVerificationId,
                ButtonStyle.Secondary,
                new Emoji("👑")
            );

        await message.ModifyAsync(properties =>
        {
            properties.Content = message.Content;
            properties.Components = builder.Build();
        });
    }

    public async Task RemoveButtonsFromMessage(IUserMessage target)
    {
        await target.ModifyAsync(properties => properties.Content = target.Content);
    }
}
