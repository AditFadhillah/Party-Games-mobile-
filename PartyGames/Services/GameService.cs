using PartyGames.Models;

namespace PartyGames.Services
{
    /// <summary>
    /// Service for managing game logic and operations
    /// </summary>
    public interface IGameService
    {
        // Dice Game
        int[] RollDice(int count);
        
        // Coin Flip
        bool FlipCoin();
        
        // Generic game operations
        List<Game> GetAllGames();
    }

    public class GameService : IGameService
    {
        private readonly Random _random = new Random();
        private List<GameScore> _sessionScores = new();

        public GameService()
        {
        }

        /// <summary>
        /// Simulates rolling the given number of six-sided dice
        /// </summary>
        public int[] RollDice(int count)
        {
            var values = new int[count];
            for (int i = 0; i < count; i++)
                values[i] = _random.Next(1, 7);
            return values;
        }

        /// <summary>
        /// Simulates a coin flip (true = heads, false = tails)
        /// </summary>
        public bool FlipCoin()
        {
            return _random.Next(0, 2) == 0;
        }

        /// <summary>
        /// Gets all available games
        /// </summary>
        public List<Game> GetAllGames()
        {
            return new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Name = "Dice Roller",
                    Description = "Roll the dice and beat your best score!",
                    IconName = "dice_icon",
                    GameType = "dice"
                },
                new Game
                {
                    Id = 2,
                    Name = "Blackjack",
                    Description = "Classic card game vs dealer",
                    IconName = "blackjack_icon",
                    GameType = "blackjack"
                },
                new Game
                {
                    Id = 3,
                    Name = "Coin Flip",
                    Description = "Call heads or tails - test your luck!",
                    IconName = "coin_icon",
                    GameType = "coinflip"
                }
            };
        }
    }
}
