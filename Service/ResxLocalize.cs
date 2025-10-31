using System.Globalization;
using System.Resources;
using ConsoleApp1.Interface;

namespace ConsoleApp1.Services;

public class ResxLocalize : IntLocalize
{
    private readonly ResourceManager manager;

    //Language is chosen here
    public ResxLocalize(string lang)
    {
        CultureInfo culture;

        if (lang == "ru")
        {
            culture = new CultureInfo("ru");
        }
        else
        {
            culture = new CultureInfo("en");
        }

        CultureInfo.CurrentUICulture = culture;
        manager = new ResourceManager("ConsoleApp1.Resources.Resources", typeof(ResxLocalize).Assembly);
    }

    //Access to a string via key
    public string this[string key]
    {
        get
        {
            string? value = manager.GetString(key);

            if (value != null)
            {
                return value;
            }
            else
            {
                return $"[{key}]";
            }
        }
    }
}
