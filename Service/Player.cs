namespace ConsoleApp1.Services
{
    //This class is an example how data will be saved
    public class Player
    {
        //Creates fields "Name" and "Wins" for data
        public string Name { get; set; }
        public int Wins { get; set; }

        public Player(string name)
        {
            Name = name;
            Wins = 0;
        }
    }
}
