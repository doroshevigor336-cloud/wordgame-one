namespace ConsoleApp1.Interface;

public interface IInput
{
    Task<string?> WordInput(int timeoutMs);
}