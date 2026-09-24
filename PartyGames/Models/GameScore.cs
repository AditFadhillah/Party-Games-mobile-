namespace PartyGames.Models
{
    /// <summary>
    /// Represents a single game score entry
    /// </summary>
    public class GameScore
    {
        public int Id { get; set; }
        public string GameName { get; set; } = string.Empty;
        public int PlayerScore { get; set; }
        public bool IsWin { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
        public string GameType { get; set; } = string.Empty; // "Dice", "Blackjack", "CoinFlip"
    }
}
