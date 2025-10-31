using System.Timers;
using ConsoleApp1.Interface;

namespace ConsoleApp1.Services;

public class ConsInput : IntInput
{
    //'timesup' is used to check if time has elapsed
    private bool timesup = false;

    public async Task<string?> WordInput(int timeoutMs)
    {
        timesup = false;

        //Timer setup
        //'timeoutMs' equals the amount of time (15000 ms by default)

        var timer1 = new System.Timers.Timer(timeoutMs);
        timer1.Elapsed += OnTimeElapsed;
        timer1.AutoReset = false;
        timer1.Start();

        //'Task.Run()' is used to end the game during input if time has elapsed

        Task<string?> input = Task.Run(() => Console.ReadLine());
        Task ends = await Task.WhenAny(input, Task.Delay(timeoutMs));

        //Stops the timer and resets it

        timer1.Stop();
        timer1.Dispose();

        //When 'Task.Run()' is completed, checks if there is no more time left

        if (ends == input)
        {
            if (!timesup)
            {
                return await input;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }


    //This event tells if there is no more time left
    private void OnTimeElapsed(object? sender, ElapsedEventArgs e)
    {
        timesup = true;
    }
}
