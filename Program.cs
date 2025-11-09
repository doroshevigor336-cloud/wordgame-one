using System;
using System.Threading.Tasks;
using ConsoleApp1.Interface;
using ConsoleApp1.Services;

class Program
{
    static async Task Main()
    {
        var output = new ConsoleOutput();
        var input = new ConsoleInput();

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
        output.WriteLine(dict["begin"]);
        string? firstWord = Console.ReadLine()?.Trim().ToLower();

        //Checks if the first word has a correct length
        if (string.IsNullOrWhiteSpace(firstWord))
        {
            output.WriteLine(dict["errlen"]);
            output.WriteLine(dict["tech"]);
            return;
        }
        else if (firstWord.Length < 8 || firstWord.Length > 30)
        {
            output.WriteLine(dict["errlen"]);
            output.WriteLine(dict["tech"]);
            return;
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
            output.WriteLine(dict["tech"]);
            return;
        }

        var checker = new WordCheck(firstWord, firstLetter, alphabetSize);
        var game = new GameCore(input, output, dict, checker, firstWord, firstLetter, alphabetSize);
        await game.Run();
    }
}
