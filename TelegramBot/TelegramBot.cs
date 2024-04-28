﻿using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using ResourceHandler.Resources;
using ResourceHandler.Resources.Models.TelegramBot;
using System.ComponentModel;
using ResourceHandler.Resources.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public class TelegramBot
{
    private static TelegramBotClient? botClient;
    private string[]? scoreStrings;
    private string? allScoresString;
    private LeaderboardManager leaderboardManager = new LeaderboardManager();

    [STAThread]
    static void Main()
    {
        try
        {
            TelegramBot bot = new TelegramBot();
            bot.Start();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Start()
    {
        botClient = new TelegramBotClient("7124354633:AAGDAxr3kPc1VDtt4PRk4nXe_UmcCZvNwLA"); // long boi bot key

        leaderboardManager = new LeaderboardManager();

        CancellationTokenSource cts;
        cts = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new UpdateType[]
           {
                    UpdateType.Message
           }
        };

        botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cancellationToken: cts.Token);

        Console.WriteLine("Bot is running. Press any key to exit.");
        do
        {
            while (!Console.KeyAvailable)
            {
                // Do something
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }

    private async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken token)
    {

        if (update.Type != UpdateType.Message)
            return;
        if (update.Message!.Type != MessageType.Text)
            return;


        long chatId = update.Message.Chat.Id;
        ProjectInitializer.Config.Chat_Id = chatId;

        if (!ProjectInitializer.Config.Bot_Status) await bot.SendTextMessageAsync(chatId, "I'm sorry, I can't help you at the moment.");
        else
        {

            CommandModel commandModel = CheckMessage(update.Message.Text);
            if (!commandModel.CommandIsAvailable) { await bot.SendTextMessageAsync(chatId, "I'm sorry, I couldn't understand you. Please type /help for my current command list."); return; }

            string? username = update.Message.Chat.Username;

            if (Enum.TryParse(commandModel.Command.ToUpper(), out Enums.Commands enumCommand) && Enum.IsDefined(typeof(Enums.Commands), enumCommand))
            {
                switch (enumCommand)
                {
                    case Enums.Commands.SCORES:
                        {
                            allScoresString = "";
                            scoreStrings = leaderboardManager.LoadEntries();

                            for (int i = 0; i < scoreStrings.Length; i++)
                            {
                                allScoresString += scoreStrings[i];
                                allScoresString += System.Environment.NewLine;
                            }
                            
                            await bot.SendTextMessageAsync(chatId, allScoresString);
                        }
                        break;
                    case Enums.Commands.ERROR:
                        {
                            await bot.SendTextMessageAsync(chatId + 1, "This will generate Error (:");
                        }
                        break;
                    case Enums.Commands.HELP:
                        {
                            await bot.SendTextMessageAsync(chatId, "/scores : Shows current high scores for the long boi game" + 
                                "\n /error : This is Development Test Purposed Command. No Need To Use It." +
                                "\n /help : Get Information About The Commands.");
                        }
                        break;
                    case Enums.Commands.HELLO:
                        {
                            await bot.SendTextMessageAsync(chatId, "Hello " + username + " !!");
                        }
                        break;
                    case Enums.Commands.TODAY:
                        {
                            await bot.SendTextMessageAsync(chatId, "Today doesn't mean anything. The Important thing is your FUTURE !!");

                        }
                        break;
                    default:
                        {
                            await bot.SendTextMessageAsync(chatId, "I'm sorry, I couldn't understand you. Please type /help for my current command list.");
                        }
                        break;
                }
            }
        }
    }

    private CommandModel CheckMessage(string? commandString)
    {
        CommandModel commandModel = new();

        commandModel.CommandText = commandString;
        commandModel.CommandSections = PrepareCommandSections(commandString);
        commandModel.CommandIsAvailable = CheckIfCommandAvailable(commandModel.CommandSections);
        commandModel.Command = commandModel.CommandIsAvailable ? commandModel.CommandSections[1] : String.Empty;
        commandModel.CommandHasParameter = commandModel.CommandIsAvailable && commandModel.CommandSections.Length > 2;
        commandModel.CommandParameter = commandModel.CommandHasParameter ? commandModel.CommandSections[2] : String.Empty;

        return commandModel;
    }

    private bool CheckIfCommandAvailable(string[] commandSections)
    {
        return ProjectInitializer.Config.Available_Commands.Contains(commandSections[1]);
    }

    private string[] PrepareCommandSections(string commandString)
    {
        var returnArray = new List<string>();
        returnArray.Add(commandString.Substring(0, 1));
        returnArray.AddRange(commandString.Substring(1).Split(' '));

        return returnArray.ToArray();
    }

    private static Task ErrorHandler(ITelegramBotClient bot, Exception exception, CancellationToken token)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Error : \n {apiRequestException.ErrorCode} \n {apiRequestException.Message}",
            _ => exception.ToString()
        };

        var chatId = ProjectInitializer.Config.Chat_Id;
        bot.SendTextMessageAsync(chatId, errorMessage);

        return Task.CompletedTask;

    }




}