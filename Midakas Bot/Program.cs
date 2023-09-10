
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Midakas_Bot;
using Midakas_Bot.Modules;
using Newtonsoft.Json;
using System;
using System.Text.Json;

public class Program
{
    private DiscordSocketClient _client;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        string text = File.ReadAllText(@"./config.json");
        var config = System.Text.Json.JsonSerializer.Deserialize<Config>(text);

        _client = new DiscordSocketClient();

        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, config.TOKEN);
        await _client.StartAsync();

        _client.SlashCommandExecuted += SlashCommandHandler;
        _client.Ready += Client_Ready;
        _client.MessageUpdated += MessageUpdated;
        _client.Ready += () =>
        {
            Console.WriteLine("Bot is connected!");
            return Task.CompletedTask;
        };

        var commandService = new CommandService();

        var commandHandler = new CommandHandler( _client, commandService);
        await commandHandler.InstallCommandsAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    public async Task Client_Ready()
    {
        var commandBuilder = new CommandBuilder();
        await commandBuilder.RunCommandBuilderAsync(_client);
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch(command.CommandName)
        {
            case "help":
                await Help.HelpCommand(command, _client);
                break;
        }
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
    {
        // If the message was not in the cache, downloading it will result in getting a copy of `after`.
        var message = await before.GetOrDownloadAsync();
        Console.WriteLine($"{message} -> {after}");
    }
}
