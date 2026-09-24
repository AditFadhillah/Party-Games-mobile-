using PartyGames.Models;

namespace PartyGames.Services
{
    /// <summary>
    /// Service for managing game scores
    /// In a real app, this would use SQLite or a database
    /// For now, it uses in-memory storage for the session
    /// </summary>
    public interface IScoreService
    {
        Task SaveScoreAsync(GameScore score);
        Task<List<GameScore>> GetScoresAsync(string gameType = "");
        Task<List<GameScore>> GetHighScoresAsync(int count = 10);
        Task ClearScoresAsync();
    }

    public class ScoreService : IScoreService
    {
        private List<GameScore> _scores = new();

        public ScoreService()
        {
            // Initialize with some sample data
        }

        public Task SaveScoreAsync(GameScore score)
        {
            score.Id = _scores.Count + 1;
            _scores.Add(score);
            return Task.CompletedTask;
        }

        public Task<List<GameScore>> GetScoresAsync(string gameType = "")
        {
            if (string.IsNullOrEmpty(gameType))
                return Task.FromResult(_scores);

            var filtered = _scores.Where(s => s.GameType == gameType).ToList();
            return Task.FromResult(filtered);
        }

        public Task<List<GameScore>> GetHighScoresAsync(int count = 10)
        {
            var highScores = _scores
                .OrderByDescending(s => s.PlayerScore)
                .Take(count)
                .ToList();

            return Task.FromResult(highScores);
        }

        public Task ClearScoresAsync()
        {
            _scores.Clear();
            return Task.CompletedTask;
        }
    }
}
