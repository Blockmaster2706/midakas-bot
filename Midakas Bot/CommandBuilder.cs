﻿using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midakas_Bot
{
    internal class CommandBuilder
    {
        internal async Task RunCommandBuilderAsync(DiscordSocketClient _client) 
        {
            string text = File.ReadAllText(@"./config.json");
            var config = System.Text.Json.JsonSerializer.Deserialize<Config>(text);

            List<SlashCommandBuilder> slashCommandBuilders = new List<SlashCommandBuilder>();

            var guild = _client.GetGuild(Convert.ToUInt64(config.GUILDID));

            var helpCommand = new SlashCommandBuilder();
            helpCommand.WithName("help");
            helpCommand.WithDescription("Dies ist der Befehl um allgemeine Hilfeinfos zum Bot zu erhalten.");
            slashCommandBuilders.Add(helpCommand);

            try
            {
                var globalCommands = await _client.GetGlobalApplicationCommandsAsync();
                
                foreach(SocketApplicationCommand command in globalCommands)    
                {
                    await command.DeleteAsync();
                }

                var guildCommands = await guild.GetApplicationCommandsAsync();

                foreach (SocketApplicationCommand command in guildCommands)
                {
                    await command.DeleteAsync();
                }

                //await guild.CreateApplicationCommandAsync(guildCommand.Build());

                await Task.Delay(500);

                // With global commands we don't need the guild.
                foreach(SlashCommandBuilder commandBuilder in slashCommandBuilders)
                {
                    await _client.CreateGlobalApplicationCommandAsync(commandBuilder.Build());

                }
                // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
            }
            catch (ApplicationCommandException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(json);
            }
        }
    }

        
}
