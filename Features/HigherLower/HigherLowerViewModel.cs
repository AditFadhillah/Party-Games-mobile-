using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
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

        protected static readonly string MonoFont = DeviceInfo.Platform == DevicePlatform.Android
            ? "monospace"
            : DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst
                ? "Menlo"
                : "Consolas";

        protected enum GuessKind { Higher, Lower, Straight }

        protected readonly record struct GuessOutcome(bool Correct, string Message, int Scorer, int Points, string? Emphasis = null);

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
        private FormattedString _statusText = new();
        private Card _showcaseCard = new(1, CardSuit.Spades) { IsFaceDown = true };

        public HigherLowerViewModel()
        {
            Title = "Higher or Lower";

            BeginCommand = new Command(async () => await BeginAsync(), () => CanBegin);
            HigherCommand = new Command(async () => await GuessAsync(GuessKind.Higher), () => CanGuess);
            LowerCommand = new Command(async () => await GuessAsync(GuessKind.Lower), () => CanGuess);
            StraightCommand = new Command(async () => await GuessAsync(GuessKind.Straight), () => CanGuess);
            PlayAgainCommand = new Command(Reset, () => IsGameOver);

            Reset();
        }

        public virtual string HeaderText => "HIGHER OR LOWER";

        public virtual string FontFamilyName => string.Empty;

        public virtual Microsoft.Maui.TextAlignment StatusAlignment => Microsoft.Maui.TextAlignment.Center;

        protected virtual string StartPrompt => "Tap the stack to start";

        protected virtual string GameOverLine => "Game Over";

        public virtual Color HigherFill => Color.FromArgb("#FF6698E8");

        public virtual Color StraightFill => Color.FromArgb("#FFFFE44C");

        public virtual Color LowerFill => Color.FromArgb("#FFFF5263");

        public virtual Color PlayAgainFill => Color.FromArgb("#FFD700");

        public virtual Color ButtonBorder => Color.FromArgb("#FFA500");

        public virtual Color ButtonText => Color.FromArgb("#333333");

        public virtual Color PlayAgainBorder => Colors.Transparent;

        public virtual double PlayAgainBorderWidth => 0;

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
        public Command StraightCommand { get; }
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

        public FormattedString StatusText
        {
            get => _statusText;
            private set => SetProperty(ref _statusText, value);
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
                StatusText = BuildStatus(PromptLine(CurrentPlayer));
                IsPlaying = true;
            }
            finally
            {
                IsAnimating = false;
            }
        }

        private async Task GuessAsync(GuessKind guess)
        {
            if (!CanGuess)
                return;

            IsAnimating = true;
            try
            {
                var previous = ShowcaseCard;
                await MoveToDiscardAsync(previous);

                var next = await DrawNextAsync();
                var outcome = Judge(guess, previous, next);

                await Task.Delay(ResolveDelayMs);

                AddPoints(outcome.Scorer, outcome.Points);

                string result = outcome.Message;

                if (_deck.Count == 0)
                {
                    EndGame($"{result}\n\n{GameOverLine}", outcome.Emphasis);
                    return;
                }

                CurrentPlayer = CurrentPlayer == 1 ? 2 : 1;
                StatusText = BuildStatus($"{result}\n\n{PromptLine(CurrentPlayer)}", outcome.Emphasis);
            }
            finally
            {
                IsAnimating = false;
            }
        }

        protected int OtherPlayer => CurrentPlayer == 1 ? 2 : 1;

        protected virtual string PromptLine(int player) => $"Player {player}, Higher or Lower?";

        /// <summary>
        /// Decides whether the guess was correct and builds the result text.
        /// Called before the turn passes, so CurrentPlayer is the guesser.
        /// </summary>
        protected virtual GuessOutcome Judge(GuessKind guess, Card previous, Card next)
        {
            bool correct = guess switch
            {
                GuessKind.Higher => Strength(next) > Strength(previous),
                GuessKind.Lower => Strength(next) < Strength(previous),
                _ => Strength(next) == Strength(previous)
            };

            string result = correct
                ? $"Correct guess for Player {CurrentPlayer}!"
                : $"Wrong guess for Player {CurrentPlayer}!";

            return new GuessOutcome(correct, result, correct ? CurrentPlayer : OtherPlayer, 1);
        }

        private void AddPoints(int player, int points)
        {
            if (player == 1)
                ScoreOne += points;
            else
                ScoreTwo += points;
        }

        private FormattedString BuildStatus(string message, string? emphasis = null)
        {
            var text = new FormattedString();
            int start = emphasis is null ? -1 : message.IndexOf(emphasis, StringComparison.Ordinal);

            if (start < 0)
            {
                text.Spans.Add(new Span { Text = message, FontFamily = FontFamilyName });
                return text;
            }

            text.Spans.Add(new Span { Text = message[..start], FontFamily = FontFamilyName });
            text.Spans.Add(new Span { Text = emphasis, TextColor = Colors.Orange, FontAttributes = FontAttributes.Bold, FontFamily = FontFamilyName });
            text.Spans.Add(new Span { Text = message[(start + emphasis!.Length)..], FontFamily = FontFamilyName });
            return text;
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

        private void EndGame(string message, string? emphasis = null)
        {
            IsPlaying = false;
            IsGameOver = true;
            StatusText = BuildStatus(message, emphasis);
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
            StatusText = BuildStatus(StartPrompt);
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
        protected static int Strength(Card card) => card.Rank == 1 ? 14 : card.Rank;

        private void RaiseCommandStates()
        {
            BeginCommand.ChangeCanExecute();
            HigherCommand.ChangeCanExecute();
            LowerCommand.ChangeCanExecute();
            StraightCommand.ChangeCanExecute();
            PlayAgainCommand.ChangeCanExecute();
        }
    }
}
