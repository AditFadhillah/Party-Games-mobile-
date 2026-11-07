using PartyGames.Models;

namespace PartyGames.Services
{
    /// <summary>
    /// Service for managing game logic and operations
    /// </summary>
    public interface IGameService
    {
        // Dice Game
        int[] RollDice(int count, int sides);
        
        // Coin Flip
        bool FlipCoin();
        
        // Generic game operations
        List<Game> GetAllGames();
    }

    public class GameService : IGameService
    {
        private readonly Random _random = new Random();

        public GameService()
        {
        }

        /// <summary>
        /// Simulates rolling the given number of dice, each with the given number of sides
        /// </summary>
        public int[] RollDice(int count, int sides)
        {
            var values = new int[count];
            for (int i = 0; i < count; i++)
                values[i] = _random.Next(1, sides + 1);
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
                    GameType = "dice"
                },
                new Game
                {
                    Id = 2,
                    Name = "Blackjack",
                    GameType = "blackjack"
                },
                new Game
                {
                    Id = 3,
                    Name = "Coin Flip",
                    GameType = "coinflip"
                },
                new Game
                {
                    Id = 4,
                    Name = "Tic-Tac-Toe",
                    GameType = "tictactoe"
                },
                new Game
                {
                    Id = 5,
                    Name = "Higher or Lower",
                    GameType = "higherlower"
                },
                new Game
                {
                    Id = 6,
                    Name = "Higher or Lower +",
                    GameType = "higherlowerplus"
                }
            };
        }
    }
}
