namespace PartyGames.Models
{
    /// <summary>
    /// Represents a player in the game
    /// </summary>
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Player";
        public int TotalScore { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
