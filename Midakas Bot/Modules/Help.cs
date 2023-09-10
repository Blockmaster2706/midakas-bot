using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Midakas_Bot.Modules
{
    internal static class Help
    {
        internal static async Task HelpCommand(SocketSlashCommand context, DiscordSocketClient _client) 
        {
            var embedAuthorBuilder = new EmbedAuthorBuilder();
            var embedBuilder = new EmbedBuilder();
            embedBuilder.AddField("Hilfe", "Test");
            embedBuilder.Author =  embedAuthorBuilder.WithName("Midakas Serverteam");

            await context.RespondAsync(embed: embedBuilder.Build());
        }
    }
}