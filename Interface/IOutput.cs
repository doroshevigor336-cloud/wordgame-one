namespace ConsoleApp1.Interface;

public interface IOutput
{
    void WriteLine(string message);
    void WaitForKey();
    void Clear();
}