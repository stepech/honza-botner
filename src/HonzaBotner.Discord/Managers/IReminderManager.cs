using System.Threading.Tasks;
using Discord;
using HonzaBotner.Services.Contract.Dto;

namespace HonzaBotner.Discord.Managers;

public interface IReminderManager
{
    Task<Embed> CreateDmReminderEmbedAsync(Reminder reminder);
    Task<Embed> CreateReminderEmbedAsync(Reminder reminder);
    Task<Embed> CreateExpiredReminderEmbedAsync(Reminder reminder);
    Task<Embed> CreateCanceledReminderEmbedAsync(Reminder reminder);
}
