using ConsoleApp1.Interface;

namespace ConsoleApp1.Services;

public class ConsOutput : IntOutput
{
    public void WriteLine(string message) => Console.WriteLine(message);
    public void WaitForKey() => Console.ReadKey();
    public void Clear() => Console.Clear();
}