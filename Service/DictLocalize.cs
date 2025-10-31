using ConsoleApp1.Interface;

namespace ConsoleApp1.Services;

public class DictLocalize : IntLocalize
{
    private readonly Dictionary<string, string> _dict;

    public DictLocalize(string lang)
    {
        //Dictionary contains used frases both in Russian and English languages 
        //When the language is chosen, only one correct dictionary is used for localization
        if (lang == "ru")
            _dict = new Dictionary<string, string>
            {
                //Russian language
                {"begin", "Игрок №1 вводит первое слово (длина - от 8 до 30 символов):"},
                {"select", "Выбран русский язык."},
                {"press", "Нажмите любую клавишу, чтобы продолжить."},
                {"errlen", "Задано слово неверной длины!"},
                {"errsym", "Слово содержит неверные символы!"},
                {"ctrue", "Слово подходит под условие!"},
                {"cfalse", "Но при этом слово повторяется!"},
                {"cwrong", "Задание не выполнено!"},
                {"tech", "Победил игрок №2, так как игрок №1 ввёл неверное значение!"},
                {"turn", "Ход №"},
                {"pl", ". Игрок №"},
                {"input", " вводит слово: "},
                {"timeout", "Время вышло!"},
                {"victory", "Победил игрок №"}
            };
        else
            _dict = new Dictionary<string, string>
            {
                //English language
                {"begin", "Player 1 enters the first word (expected length - from 8 to 30 symbols):"},
                {"select", "English language was chosen."},
                {"press", "Press any button to continue."},
                {"errlen", "Incorrect length!"},
                {"errsym", "Incorrect symbols!"},
                {"ctrue", "The word is correct!"},
                {"cfalse", "But the word has already been used!"},
                {"cwrong", "The word is incorrect!"},
                {"tech", "Player 2 won, because Player 1 used wrong inputs!"},
                {"turn", "Turn "},
                {"pl", ". Player "},
                {"input", " enters a word: "},
                {"timeout", "Time's up!"},
                {"victory", "Victory goes to Player "}
            };
    }
    //Connection with interface
    public string this[string key] => _dict.ContainsKey(key) ? _dict[key] : $"[{key}]";
}