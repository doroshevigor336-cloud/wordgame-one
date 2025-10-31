using ConsoleApp1.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp1.Services;

public class GameCore
{
    private readonly IntInput input;
    private readonly IntOutput output;
    private readonly IntLocalize dict;
    private readonly WordCheck checker;
    private readonly string firstword;
    private readonly char firstletter;
    private readonly int mas;

    public GameCore(IntInput input, IntOutput output, IntLocalize dict, WordCheck checker, string firstword, char firstletter, int mas)
    {
        this.input = input;
        this.output = output;
        this.dict = dict;
        this.checker = checker;
        this.firstword = firstword;
        this.firstletter = firstletter;
        this.mas = mas;
    }

    public async Task Run()
    {
    //This clears the console
    output.WriteLine(dict["press"]);
    output.WaitForKey();
    output.Clear();

    //Starts counting turns from 2
    int count = 2;

    //Creates a list to check if the word has been used or not
    List<string> words = new List<string> {firstword};

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

        output.WriteLine(dict["turn"] + count + dict["pl"] + player + dict["input"]);

        //Timer for 15 seconds
        string? secondword = await input.WordInput(15000);
        if (secondword == null)
        {
            output.WriteLine(dict["timeout"]);
            break;
        }

        //Deletes unnecessary spaces and puts all letters in lowercase 
        secondword = secondword.Trim().ToLower();
        count++;

        //Checks if the word is correct
        bool rule = checker.Check(secondword);

        //Checks if the word has already been used
        if (rule)
        {
            output.WriteLine(dict["ctrue"]);

            if (words.Contains(secondword))
            {
                output.WriteLine(dict["cfalse"]);
                break;
            }
            else
            {
                //Adds the word to the list
                words.Add(secondword);

                //Clears the console
                output.WriteLine(dict["press"]);
                output.WaitForKey();
                output.Clear();
            }
        }
        else
        {
            output.WriteLine(dict["cwrong"]);
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
    output.WriteLine(dict["victory"] + winner + "!");
    }

}   
