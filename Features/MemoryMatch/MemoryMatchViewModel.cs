using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Storage;
using PartyGames.ViewModels;

namespace PartyGames.Features.MemoryMatch
{
    public enum MemoryDifficulty { Easy, Medium, Hard }

    /// <summary>
    /// ViewModel for Memory Match: find fruit pairs against the clock. Harder levels add more cards.
    /// </summary>
    public class MemoryMatchViewModel : BaseViewModel
    {
        private const int MismatchDelayMs = 800;
        private const string BestKeyPrefix = "memory_best_";

        private static readonly string[] Fruits =
        {
            "🍎", "🍌", "🍇", "🍓", "🍒", "🍑", "🍍", "🥭", "🍉", "🍊", "🍋", "🥝"
        };

        private readonly Stopwatch _stopwatch = new();
        private readonly IDispatcherTimer _timer;
        private MemoryCard? _firstFlipped;
        private MemoryDifficulty _difficulty = MemoryDifficulty.Medium;
        private bool _isChecking;
        private bool _isWinVisible;
        private int _gameId;
        private int _matchedPairs;
        private int _totalPairs;
        private int _columns;
        private string _timeText = "00:00";
        private string _bestText = string.Empty;
        private string _statusMessage = string.Empty;
        private string _winMessage = string.Empty;

        public MemoryMatchViewModel()
        {
            Title = "Memory Match";

            _timer = Application.Current!.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += (_, _) => TimeText = FormatTime(_stopwatch.Elapsed);

            Cards = new ObservableCollection<MemoryCard>();
            FlipCommand = new Command<MemoryCard>(async card => await FlipAsync(card));
            StartCommand = new Command<string>(level =>
                StartGame(Enum.Parse<MemoryDifficulty>(level ?? nameof(MemoryDifficulty.Medium))));
            NewGameCommand = new Command(() => StartGame(_difficulty));

            StartGame(_difficulty);
        }

        public ObservableCollection<MemoryCard> Cards { get; }
        public ICommand FlipCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand NewGameCommand { get; }

        public int Columns
        {
            get => _columns;
            private set => SetProperty(ref _columns, value);
        }

        public string TimeText
        {
            get => _timeText;
            private set => SetProperty(ref _timeText, value);
        }

        public string BestText
        {
            get => _bestText;
            private set => SetProperty(ref _bestText, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetProperty(ref _statusMessage, value);
        }

        public string WinMessage
        {
            get => _winMessage;
            private set => SetProperty(ref _winMessage, value);
        }

        public bool IsWinVisible
        {
            get => _isWinVisible;
            private set => SetProperty(ref _isWinVisible, value);
        }

        /// <summary>
        /// Stops the clock. Called when the page leaves the screen.
        /// </summary>
        public void StopTimer()
        {
            _timer.Stop();
            _stopwatch.Stop();
        }

        private void StartGame(MemoryDifficulty difficulty)
        {
            StopTimer();
            _difficulty = difficulty;
            _gameId++;
            _firstFlipped = null;
            _isChecking = false;
            _matchedPairs = 0;

            var (pairs, columns) = difficulty switch
            {
                MemoryDifficulty.Easy => (6, 3),
                MemoryDifficulty.Medium => (8, 4),
                _ => (12, 4)
            };

            _totalPairs = pairs;
            Columns = columns;
            _stopwatch.Reset();
            TimeText = "00:00";
            BestText = GetBestText();
            IsWinVisible = false;
            StatusMessage = "Tap a card to start the clock";

            var symbols = Fruits.Take(pairs)
                .SelectMany(fruit => new[] { fruit, fruit })
                .OrderBy(_ => Random.Shared.Next())
                .ToList();

            Cards.Clear();
            for (int i = 0; i < symbols.Count; i++)
                Cards.Add(new MemoryCard(i, symbols[i]));
        }

        private async Task FlipAsync(MemoryCard? card)
        {
            if (card == null || _isChecking || card.IsFaceUp || card.IsMatched)
                return;

            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
                _timer.Start();
            }

            card.IsFaceUp = true;

            if (_firstFlipped == null)
            {
                _firstFlipped = card;
                StatusMessage = "Find the matching fruit";
                return;
            }

            var first = _firstFlipped;
            _firstFlipped = null;

            if (first.Emoji == card.Emoji)
            {
                first.IsMatched = true;
                card.IsMatched = true;
                _matchedPairs++;
                StatusMessage = "Match!";

                if (_matchedPairs == _totalPairs)
                    FinishGame();

                return;
            }

            var gameId = _gameId;
            _isChecking = true;
            StatusMessage = "Not a match";
            await Task.Delay(MismatchDelayMs);

            if (gameId != _gameId)
                return;

            first.IsFaceUp = false;
            card.IsFaceUp = false;
            _isChecking = false;
            StatusMessage = "Find the matching fruit";
        }

        private void FinishGame()
        {
            StopTimer();

            var elapsed = _stopwatch.Elapsed;
            TimeText = FormatTime(elapsed);

            var key = BestKeyPrefix + _difficulty;
            var best = Preferences.Default.Get(key, -1d);
            bool newRecord = best < 0 || elapsed.TotalSeconds < best;

            if (newRecord)
                Preferences.Default.Set(key, elapsed.TotalSeconds);

            BestText = GetBestText();
            WinMessage = newRecord
                ? $"New best time: {FormatTime(elapsed)}!"
                : $"You finished in {FormatTime(elapsed)}";
            StatusMessage = "All pairs found!";
            IsWinVisible = true;
        }

        private string GetBestText()
        {
            var best = Preferences.Default.Get(BestKeyPrefix + _difficulty, -1d);
            return best < 0 ? "Best: --" : $"Best: {FormatTime(TimeSpan.FromSeconds(best))}";
        }

        private static string FormatTime(TimeSpan time) =>
            $"{(int)time.TotalMinutes:00}:{time.Seconds:00}";
    }
}
