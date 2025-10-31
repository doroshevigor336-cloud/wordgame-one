namespace ConsoleApp1.Interface;

public interface IntInput
{
    Task<string?> WordInput(int timeoutMs);
}