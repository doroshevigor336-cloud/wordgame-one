using ConsoleApp1.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp1.Services;

public class GameCore
{
    private readonly IInput _input;
    private readonly IOutput _output;
    private readonly ILocalize _localization;
    private readonly WordCheck _checker;
    private readonly string _firstword;
    private readonly char _firstletter;
    private readonly int _mas;
    private readonly PlayerScore _playerScore;
    private readonly List<Player> _players;
    private readonly GameStateService _stateService = new();

    public GameCore(
        IInput input,
        IOutput output,
        ILocalize localization,
        WordCheck checker,
        string firstword,
        char firstletter,
        int mas,
        PlayerScore playerScore,
        List<Player> players)
    {
        _input = input;
        _output = output;
        _localization = localization;
        _checker = checker;
        _firstword = firstword;
        _firstletter = firstletter;
        _mas = mas;
        _playerScore = playerScore;
        _players = players;
    }

    public async Task Run()
    {
        //This clears the console
        _output.WriteLine(_localization["press"]);
        _output.WaitForKey();
        _output.Clear();

        //Starts counting turns from 2
        int count = 2;

        //Creates a list to check if the word has been used or not
        List<string> words = new List<string> { _firstword };

        _stateService.SaveState(count, words, _players);

        //A loop for new words
        while (true)
        {
            //Player count
            int player;
            if (count % 2 == 0)
            {
                player = 2;
            }
            else
            {
                player = 1;
            }

            _output.WriteLine(_localization["turn"] + count + _localization["pl"] + player + _localization["input"]);

            //Timer for 15 seconds
            string? secondword = await _input.WordInput(15000);
            if (secondword == null)
            {
                _output.WriteLine(_localization["timeout"]);
                _stateService.SaveState(count, words, _players);
                break;
            }

            //Deletes unnecessary spaces and puts all letters in lowercase 
            secondword = secondword.Trim().ToLower();

            //Checks if the player tries to access a word list
            if (secondword == "/show-words")
            {
                _output.WriteLine(_localization["stopGame"]);

                _output.WriteLine(_localization["words"]);
                foreach (var w in words)
                {
                    _output.WriteLine(w);
                }

                _output.WriteLine(_localization["press"]);
                _output.WaitForKey();
                _output.Clear();

                continue;
            }

            //Checks if the player tries to access a scoreboard
            if (secondword == "/score")
            {
                _output.WriteLine(_localization["stopGame"]);

                _output.WriteLine(_localization["players"]);
                foreach (var p in _players)
                {
                    _output.WriteLine($"{p.Name}" + _localization["wins"] + $"{p.Wins}");
                }

                _output.WriteLine(_localization["press"]);
                _output.WaitForKey();
                _output.Clear();

                continue;
            }

            //Checks if the player tries to access a total scoreboard   
            if (secondword == "/total-score")
            {
                _output.WriteLine(_localization["stopGame"]);

                var allPlayers = _playerScore.LoadPlayers();

                _output.WriteLine(_localization["allPlayers"]);
                foreach (var p in allPlayers)
                {
                    _output.WriteLine($"{p.Name}" + _localization["wins"] + $"{p.Wins}");
                }

                _output.WriteLine(_localization["press"]);
                _output.WaitForKey();
                _output.Clear();

                continue;
            }

            count++;

            //Checks if the word is correct
            bool rule = _checker.Check(secondword);

            //Checks if the word has already been used
            if (rule)
            {
                _output.WriteLine(_localization["ctrue"]);

                if (words.Contains(secondword))
                {
                    _output.WriteLine(_localization["cfalse"]);
                    _stateService.SaveState(count, words, _players);
                    break;
                }
                else
                {
                    //Adds the word to the list
                    words.Add(secondword);

                    _stateService.SaveState(count, words, _players);

                    //Clears the console
                    _output.WriteLine(_localization["press"]);
                    _output.WaitForKey();
                    _output.Clear();
                }
            }
            else
            {
                _output.WriteLine(_localization["cwrong"]);
                _stateService.SaveState(count, words, _players);
                break;
            }
        }

        //Checks who is the winner
        int winnerIndex;
        if (count % 2 == 1)
        {
            winnerIndex = 1;
        }
        else
        {
            winnerIndex = 2;
        }

        Player winner = _players[winnerIndex - 1];
        _playerScore.AddWin(winner);
        _playerScore.SavePlayers(_players);

        //Deletes the game file because its not needed
        _stateService.ClearState();

        //Tells who the winner is and ends the game
        _output.WriteLine(_localization["victory"] + winnerIndex + " (" + winner.Name + ")" + "!");
    }
}
