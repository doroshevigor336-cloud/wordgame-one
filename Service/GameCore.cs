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

    public GameCore(IInput input, IOutput output, ILocalize localization, WordCheck checker, string firstword, char firstletter, int mas)
    {
        _input = input;
        _output = output;
        _localization = localization;
        _checker = checker;
        _firstword = firstword;
        _firstletter = firstletter;
        _mas = mas;
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
    List<string> words = new List<string> {_firstword};

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
            break;
        }

        //Deletes unnecessary spaces and puts all letters in lowercase 
        secondword = secondword.Trim().ToLower();
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
                break;
            }
            else
            {
                //Adds the word to the list
                words.Add(secondword);

                //Clears the console
                _output.WriteLine(_localization["press"]);
                _output.WaitForKey();
                _output.Clear();
            }
        }
        else
        {
            _output.WriteLine(_localization["cwrong"]);
            break;
        }
    }

    //Checks who is the winner
    int winner;
    if (count % 2 == 0)
    {
        winner = 2;
    }
    else
    {
        winner = 1;
    }

    //Tells who the winner is and ends the game
    _output.WriteLine(_localization["victory"] + winner + "!");
    }

}   
