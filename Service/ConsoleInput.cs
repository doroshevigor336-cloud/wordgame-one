using System.Timers;
using ConsoleApp1.Interface;

namespace ConsoleApp1.Services;

public class ConsoleInput : IInput
{
    //'timesup' is used to check if time has elapsed
    private bool _timeIsUp = false;

    public async Task<string?> WordInput(int timeoutMs)
    {
        _timeIsUp = false;

        //Timer setup
        //'timeoutMs' equals the amount of time (15000 ms by default)

        var timer = new System.Timers.Timer(timeoutMs);
        timer.Elapsed += OnTimeElapsed;
        timer.AutoReset = false;
        timer.Start();

        //'Task.Run()' is used to end the game during input if time has elapsed

        Task<string?> input = Task.Run(() => Console.ReadLine());
        Task ends = await Task.WhenAny(input, Task.Delay(timeoutMs));

        //Stops the timer and resets it

        timer.Stop();
        timer.Dispose();

        //When 'Task.Run()' is completed, checks if there is no more time left

        if (ends == input)
        {
            if (!_timeIsUp)
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
        _timeIsUp = true;
    }
}
