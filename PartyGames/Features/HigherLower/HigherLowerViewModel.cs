using System.Collections.ObjectModel;
using PartyGames.Features.Blackjack;
using PartyGames.ViewModels;

namespace PartyGames.Features.HigherLower
{
    /// <summary>
    /// Two-player Higher or Lower through one shuffled 52-card deck. Ace is high.
    /// </summary>
    public class HigherLowerViewModel : BaseViewModel
    {
        private const int ResolveDelayMs = 600;
        private const string PromptBegin = "Tap the stack to start";

        private readonly Random _random = new();
        private readonly List<Card> _deck = new();

        private int _scoreOne;
        private int _scoreTwo;
        private int _currentPlayer = 1;
        private bool _isPlaying;
        private bool _isAnimating;
        private bool _isGameOver;
        private double _stackOpacity = 1;
        private double _showcaseOpacity;
        private string _statusMessage = PromptBegin;
        private Card _showcaseCard = new(1, CardSuit.Spades) { IsFaceDown = true };

        public HigherLowerViewModel()
        {
            Title = "Higher or Lower";

            BeginCommand = new Command(async () => await BeginAsync(), () => CanBegin);
            HigherCommand = new Command(async () => await GuessAsync(higher: true), () => CanGuess);
            LowerCommand = new Command(async () => await GuessAsync(higher: false), () => CanGuess);
            PlayAgainCommand = new Command(Reset, () => IsGameOver);

            Reset();
        }

        public ObservableCollection<Card> Discard { get; } = new();

        /// <summary>
        /// Flies the showcase card from the stack and shakes it. Set by the view.
        /// </summary>
        public Func<Task>? DrawAnimationAsync { get; set; }

        /// <summary>
        /// Flies the showcase card onto the discard pile. Set by the view.
        /// </summary>
        public Func<Task>? DiscardAnimationAsync { get; set; }

        public Command BeginCommand { get; }
        public Command HigherCommand { get; }
        public Command LowerCommand { get; }
        public Command PlayAgainCommand { get; }

        public int CurrentPlayer
        {
            get => _currentPlayer;
            private set => SetProperty(ref _currentPlayer, value);
        }

        public int ScoreOne
        {
            get => _scoreOne;
            private set => SetProperty(ref _scoreOne, value);
        }

        public int ScoreTwo
        {
            get => _scoreTwo;
            private set => SetProperty(ref _scoreTwo, value);
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            private set
            {
                if (SetProperty(ref _isPlaying, value))
                    RaiseCommandStates();
            }
        }

        public bool IsAnimating
        {
            get => _isAnimating;
            private set
            {
                if (SetProperty(ref _isAnimating, value))
                    RaiseCommandStates();
            }
        }

        public bool IsGameOver
        {
            get => _isGameOver;
            private set
            {
                if (SetProperty(ref _isGameOver, value))
                    RaiseCommandStates();
            }
        }

        public Card ShowcaseCard
        {
            get => _showcaseCard;
            private set => SetProperty(ref _showcaseCard, value);
        }

        public double ShowcaseOpacity
        {
            get => _showcaseOpacity;
            private set => SetProperty(ref _showcaseOpacity, value);
        }

        public double StackOpacity
        {
            get => _stackOpacity;
            private set => SetProperty(ref _stackOpacity, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetProperty(ref _statusMessage, value);
        }

        public bool CanBegin => !IsPlaying && !IsAnimating && !IsGameOver;

        public bool CanGuess => IsPlaying && !IsAnimating;

        private async Task BeginAsync()
        {
            if (!CanBegin)
                return;

            IsAnimating = true;
            try
            {
                await DrawNextAsync();
                StatusMessage = $"Player {CurrentPlayer}, Higher or Lower?";
                IsPlaying = true;
            }
            finally
            {
                IsAnimating = false;
            }
        }

        private async Task GuessAsync(bool higher)
        {
            if (!CanGuess)
                return;

            IsAnimating = true;
            try
            {
                var previous = ShowcaseCard;
                await MoveToDiscardAsync(previous);

                var next = await DrawNextAsync();
                bool correct = higher
                    ? Strength(next) > Strength(previous)
                    : Strength(next) < Strength(previous);

                await Task.Delay(ResolveDelayMs);

                AddPoint(correct ? CurrentPlayer : OtherPlayer);

                string result = correct
                    ? $"Correct guess for Player {CurrentPlayer}!"
                    : $"Wrong guess for Player {CurrentPlayer}!";

                if (_deck.Count == 0)
                {
                    EndGame($"{result}\n\n{GameResult()}");
                    return;
                }

                CurrentPlayer = CurrentPlayer == 1 ? 2 : 1;
                StatusMessage = $"{result}\n\nPlayer {CurrentPlayer}, Higher or Lower?";
            }
            finally
            {
                IsAnimating = false;
            }
        }

        private int OtherPlayer => CurrentPlayer == 1 ? 2 : 1;

        private void AddPoint(int player)
        {
            if (player == 1)
                ScoreOne++;
            else
                ScoreTwo++;
        }

        private string GameResult()
        {
            if (ScoreOne == ScoreTwo)
                return $"Draw, {ScoreOne}-{ScoreTwo}.";

            int winner = ScoreOne > ScoreTwo ? 1 : 2;
            return $"Player {winner} wins, {Math.Max(ScoreOne, ScoreTwo)}-{Math.Min(ScoreOne, ScoreTwo)}.";
        }

        private async Task MoveToDiscardAsync(Card card)
        {
            if (DiscardAnimationAsync is not null)
                await DiscardAnimationAsync();

            Discard.Add(card);
            ShowcaseOpacity = 0;
        }

        private async Task<Card> DrawNextAsync()
        {
            var card = _deck[^1];
            _deck.RemoveAt(_deck.Count - 1);

            card.IsFaceDown = true;
            ShowcaseCard = card;
            ShowcaseOpacity = 1;

            if (DrawAnimationAsync is not null)
                await DrawAnimationAsync();

            card.IsFaceDown = false;
            UpdateStack();
            return card;
        }

        private void EndGame(string message)
        {
            IsPlaying = false;
            IsGameOver = true;
            StatusMessage = message;
        }

        private void Reset()
        {
            BuildShuffledDeck();
            Discard.Clear();
            ScoreOne = 0;
            ScoreTwo = 0;
            CurrentPlayer = 1;
            IsPlaying = false;
            IsGameOver = false;
            ShowcaseOpacity = 0;
            ShowcaseCard = new Card(1, CardSuit.Spades) { IsFaceDown = true };
            StatusMessage = PromptBegin;
            UpdateStack();
        }

        private void UpdateStack()
        {
            StackOpacity = _deck.Count > 0 ? 1 : 0;
        }

        private void BuildShuffledDeck()
        {
            _deck.Clear();

            foreach (CardSuit suit in Enum.GetValues<CardSuit>())
            {
                for (int rank = 1; rank <= 13; rank++)
                    _deck.Add(new Card(rank, suit));
            }

            for (int i = _deck.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (_deck[i], _deck[j]) = (_deck[j], _deck[i]);
            }
        }

        /// <summary>
        /// Ace is high: 14 beats King.
        /// </summary>
        private static int Strength(Card card) => card.Rank == 1 ? 14 : card.Rank;

        private void RaiseCommandStates()
        {
            BeginCommand.ChangeCanExecute();
            HigherCommand.ChangeCanExecute();
            LowerCommand.ChangeCanExecute();
            PlayAgainCommand.ChangeCanExecute();
        }
    }
}
