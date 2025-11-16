using System.Text.Json;
using ConsoleApp1.Services;


public class GameState
{
    public int CurrentTurn { get; set; }
    public List<string> Words { get; set; } = new();
    public List<Player> Players { get; set; } = new();
}

public class GameStateService
{
    private readonly string _filePath = "gamestate.json";

    //Saves data about the current game
    public void SaveState(int currentTurn, List<string> words, List<Player> players)
    {
        var state = new GameState
        {
            CurrentTurn = currentTurn,
            Words = new List<string>(words),
            Players = new List<Player>(players)
        };

        var json = JsonSerializer.Serialize(state, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        File.WriteAllText(_filePath, json);

    }

    //Loads data from a file to further check who wins
    public GameState? LoadState()
    {
        if (!File.Exists(_filePath))
            return null;

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<GameState>(json);
    }

    //Deletes previous file before a new game starts
    public void ClearState()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }
}
