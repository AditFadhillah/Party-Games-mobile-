namespace PartyGames.Models
{
    /// <summary>
    /// Represents a game available in the app
    /// </summary>
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
        public string GameType { get; set; } = string.Empty; // Used for routing/identification
        public bool IsEnabled { get; set; } = true;
    }
}
