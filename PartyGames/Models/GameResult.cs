namespace PartyGames.Models
{
    /// <summary>
    /// Represents the result of a game round
    /// </summary>
    public class GameResult
    {
        public enum ResultType { Win, Loss, Tie, Draw }

        public int Id { get; set; }
        public string GameName { get; set; } = string.Empty;
        public ResultType Result { get; set; }
        public int PlayerScore { get; set; }
        public int OpponentScore { get; set; } // For games with opponent (Blackjack)
        public string Details { get; set; } = string.Empty; // Additional game-specific details
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
