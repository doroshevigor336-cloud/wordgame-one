namespace ConsoleApp1.Interface;

public interface ILocalize
{
    string this[string key] { get; }
}