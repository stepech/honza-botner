﻿using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HonzaBotner.Discord.Services.Attributes;

namespace HonzaBotner.Discord.Services.Commands
{
    [Group("voice")]
    [Description("Commands to control custom voice channels.")]
    [RequireChannel(750108543125946448)]
    public class VoiceCommands : BaseCommandModule
    {
        private readonly IVoiceManager _voiceManager;

        public VoiceCommands(IVoiceManager voiceManager) => _voiceManager = voiceManager;

        [Command("add")]
        [Description("Create new voice channel. Users has 30 seconds to join.")]
        public Task AddVoiceChannel(
            CommandContext ctx,
            [Description("Name of the channel.")] string name,
            [Description("Limit number of members who can join.")] int limit = 0
        )
        {
            _voiceManager.AddNewVoiceChannelAsync(ctx.Client, ctx.Guild.GetChannel(810277031089930251), ctx.Member,
                name, limit);
            return ctx.RespondAsync($"I have created new voice channel '{name}' for you!");
        }

        [Command("edit")]
        [Description("Create new voice channel. Users has 30 seconds to join.")]
        public async Task EditVoiceChannel(
            CommandContext ctx,
            [Description("Name of the channel.")] string newName,
            [Description("Limit number of members who can join.")] int? limit = null
        )
        {
            if (ctx.Member.VoiceState.Channel != null && ctx.Member.VoiceState.Channel.Parent.Id == 750055929340231714)
            {
                if ((ctx.Member.VoiceState.Channel.PermissionsFor(ctx.Member) & Permissions.ManageChannels) == 0) return;

                await ctx.Member.VoiceState.Channel.ModifyAsync(model =>
                {
                    model.Name = newName;

                    if (limit != null)
                    {
                        model.Userlimit = limit;
                    }
                });
            }
        }
    }
}
