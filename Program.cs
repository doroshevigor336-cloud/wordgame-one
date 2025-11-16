using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleApp1.Interface;
using ConsoleApp1.Services;

class Program
{
    static async Task Main()
    {
        var output = new ConsoleOutput();
        var input = new ConsoleInput();

        var playerScore = new PlayerScore();
        var allPlayers = playerScore.LoadPlayers();

        var stateService = new GameStateService();
        var savedState = stateService.LoadState();

        //Checks if there was an unfinished game
        if (savedState != null)
        {
            int winnerIndex = (savedState.CurrentTurn % 2 == 0) ? 0 : 1;

            //Adds a player to the list if he is not already there
            foreach (var savedPlayer in savedState.Players)
            {
                playerScore.GetOrCreatePlayer(savedPlayer.Name, allPlayers);
            }

            //Finds a winner in the list
            Player winner = allPlayers.Find(p => p.Name.Equals(savedState.Players[winnerIndex].Name, StringComparison.OrdinalIgnoreCase));
            if (winner != null)
            {
                playerScore.AddWin(winner);
            }

            //Saves data
            playerScore.SavePlayers(allPlayers);

            stateService.ClearState();
        }


        output.Clear();

        //Gets data about names and score
        //If player doesn't exist, creates a new one
        Console.WriteLine("Игрок 1/Player 1:");
        string name1 = Console.ReadLine();
        Player player1 = playerScore.GetOrCreatePlayer(name1, allPlayers);

        Console.WriteLine("Игрок 2/Player 2:");
        string name2 = Console.ReadLine();
        Player player2 = playerScore.GetOrCreatePlayer(name2, allPlayers);
        output.Clear();

        string language = "";

        //Localization depends on user's choice, the game won't start if the input is wrong
        while (true)
        {
            output.WriteLine("Выберите язык/Choose your language");
            output.WriteLine("Напишите 'ru' для поддержки русского языка");
            output.WriteLine("Type 'en' for english language support");

            language = Console.ReadLine();
            if (language == "ru" || language == "en")
                break;

            //If the input is wrong, the game requests the input again
            output.WriteLine("Неверный ввод/Wrong input");
            output.WriteLine("Нажмите любую клавишу/Press any key");
            output.WaitForKey();
            output.Clear();
        }

        //The dictionary is connected here
        ILocalize dict = new ResxLocalize(language);

        //Correct values are used here (depends on localization)
        char firstLetter;
        int alphabetSize;

        if (language == "ru")
        {
            firstLetter = 'а';
            alphabetSize = 33;
        }
        else
        {
            firstLetter = 'a';
            alphabetSize = 26;
        }

        output.WriteLine(dict["select"]);
        output.WriteLine(dict["press"]);
        output.WaitForKey();
        output.Clear();

        //The game starts with the first word
        string? firstWord = null;

        while (true)
        {
            output.WriteLine(dict["begin"]);
            firstWord = Console.ReadLine()?.Trim().ToLower();

            //Checks if the first word has a correct length
            if (string.IsNullOrWhiteSpace(firstWord))
            {
                output.WriteLine(dict["errlen"]);
                output.WriteLine(dict["press"]);
                output.WaitForKey();
                output.Clear();
                continue;
            }
            else if (firstWord.Length < 8 || firstWord.Length > 30)
            {
                output.WriteLine(dict["errlen"]);
                output.WriteLine(dict["press"]);
                output.WaitForKey();
                output.Clear();
                continue;
            }

            //Checks if the word has correct symbols
            bool valid = true;
            foreach (char c in firstWord)
            {
                int nomer = c - firstLetter;
                if (nomer < 0 || nomer >= alphabetSize)
                {
                    valid = false;
                    break;
                }
            }

            if (!valid)
            {
                output.WriteLine(dict["errsym"]);
                output.WriteLine(dict["press"]);
                output.WaitForKey();
                output.Clear();
                continue;
            }

            break;
        }

        var checker = new WordCheck(firstWord, firstLetter, alphabetSize);
        var game = new GameCore(
            input,
            output,
            dict,
            checker,
            firstWord,
            firstLetter,
            alphabetSize,
            playerScore,
            new List<Player> { player1, player2 }
        );

        await game.Run();

        //Saves new data about players
        playerScore.SavePlayers(allPlayers);
    }
}
