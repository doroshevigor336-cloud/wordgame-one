using System;
using System.Threading.Tasks;
using ConsoleApp1.Interface;
using ConsoleApp1.Services;

class Program
{
    static async Task Main()
    {
        var output = new ConsOutput();
        var input = new ConsInput();

        output.Clear();

        string lang = "";

        //Localization depends on user's choice, the game won't start if the input is wrong
        while (true)
        {
            output.WriteLine("Выберите язык/Choose your language");
            output.WriteLine("Напишите 'ru' для поддержки русского языка");
            output.WriteLine("Type 'en' for english language support");

            lang = Console.ReadLine();
            if (lang == "ru" || lang == "en")
                break;

            //If the input is wrong, the game requests the input again
            output.WriteLine("Неверный ввод/Wrong input");
            output.WriteLine("Нажмите любую клавишу/Press any key");
            output.WaitForKey();
            output.Clear();
        }

        //The dictionary is connected here
        IntLocalize dict = new ResxLocalize(lang);

        //Correct values are used here (depends on localization)
        char firstletter;
        int mas;

        if (lang == "ru")
        {
            firstletter = 'а';
            mas = 33;
        }
        else
        {
            firstletter = 'a';
            mas = 26;
        }

        output.WriteLine(dict["select"]);
        output.WriteLine(dict["press"]);
        output.WaitForKey();
        output.Clear();

        //The game starts with the first word
        output.WriteLine(dict["begin"]);
        string? firstword = Console.ReadLine()?.Trim().ToLower();

        //Checks if the first word has a correct length
        if (string.IsNullOrWhiteSpace(firstword))
        {
            output.WriteLine(dict["errlen"]);
            output.WriteLine(dict["tech"]);
            return;
        }
        else if (firstword.Length < 8 || firstword.Length > 30)
        {
            output.WriteLine(dict["errlen"]);
            output.WriteLine(dict["tech"]);
            return;
        }

        //Checks if the word has correct symbols
        bool valid = true;
        foreach (char c in firstword)
        {
            int nomer = c - firstletter;
            if (nomer < 0 || nomer >= mas)
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

        var checker = new WordCheck(firstword, firstletter, mas);
        var game = new GameCore(input, output, dict, checker, firstword, firstletter, mas);
        await game.Run();
    }
}
