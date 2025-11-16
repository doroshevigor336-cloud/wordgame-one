using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace ConsoleApp1.Services
{
    public class PlayerScore
    {
        private readonly string _filePath;

        public PlayerScore(string filePath = "players.json")
        {
            _filePath = filePath;
        }

        //This function loads data from the file
        public List<Player> LoadPlayers()
        {
            if (!File.Exists(_filePath))
                return new List<Player>();

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Player>>(json) ?? new List<Player>();
        }

        //This function saves data (merges with existing)
        public void SavePlayers(List<Player> players)
        {
            var existingPlayers = LoadPlayers();

            foreach (var p in players)
            {
                var existing = existingPlayers.Find(x => x.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                {
                    existing.Wins = p.Wins; // обновляем счёт
                }
                else
                {
                    existingPlayers.Add(p); // добавляем нового
                }
            }

            string json = JsonSerializer.Serialize(existingPlayers, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(_filePath, json);
        }

        //This function loads data or creates a new player
        public Player GetOrCreatePlayer(string name, List<Player> players)
        {
            var existing = players.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                return existing;
            }

            var newPlayer = new Player(name);
            players.Add(newPlayer);
            return newPlayer;
        }

        //This function adds a point for the winner
        public void AddWin(Player player)
        {
            player.Wins++;
        }
    }
}
