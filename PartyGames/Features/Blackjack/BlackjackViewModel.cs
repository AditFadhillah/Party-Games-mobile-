using System.Collections.ObjectModel;
using PartyGames.ViewModels;

namespace PartyGames.Features.Blackjack
{
    /// <summary>
    /// ViewModel for a Blackjack session. Each visit starts with the starting money.
    /// </summary>
    public class BlackjackViewModel : BaseViewModel
    {
        private const int StartingMoney = 500;
        private const int BetStep = 5;
        private const int BlackjackTarget = 21;
        private const int DealerStandValue = 17;
        private const int DrawDelayMs = 400;
        private const int SettleDelayMs = 800;
        private const string ResultPlayerWin = "Player win";
        private const string ResultDealerWin = "Dealer win";
        private const string ResultDealerAdvantage = "Dealer advantage";
        private const string PromptBet = "Place your bet and deal.";

        private readonly Random _random = new();
        private readonly List<Card> _deck = new();

        private int _money = StartingMoney;
        private int _bet = BetStep;
        private bool _isRoundActive;
        private bool _isAnimating;
        private bool _isResultVisible;
        private string _resultMessage = string.Empty;
        private string _statusMessage = PromptBet;
        private string _playerTotalText = "Player: 0";
        private string _dealerTotalText = "Dealer: ?";

        public BlackjackViewModel()
        {
            Title = "Blackjack";

            IncreaseBetCommand = new Command(() => ChangeBet(BetStep), () => CanChangeBet && Bet + BetStep <= Money);
            DecreaseBetCommand = new Command(() => ChangeBet(-BetStep), () => CanChangeBet && Bet > BetStep);
            DealCommand = new Command(Deal, () => CanDeal);
            HitCommand = new Command(async () => await HitAsync(), () => CanHit);
            StandCommand = new Command(async () => await StandAsync(), () => CanStand);
            ResetCommand = new Command(Reset, () => IsOutOfMoney);
            DismissResultCommand = new Command(DismissResult);

            ShowPlaceholderHands();
        }

        public ObservableCollection<Card> PlayerHand { get; } = new();

        public ObservableCollection<Card> DealerHand { get; } = new();

        /// <summary>
        /// Shakes the hand row from the view. The bool is true for the dealer.
        /// </summary>
        public Func<bool, Task>? ShakeHandAsync { get; set; }

        public Func<int, Task>? ShowMoneyChangeAsync { get; set; }

        public Command IncreaseBetCommand { get; }
        public Command DecreaseBetCommand { get; }
        public Command DealCommand { get; }
        public Command HitCommand { get; }
        public Command StandCommand { get; }
        public Command ResetCommand { get; }
        public Command DismissResultCommand { get; }

        public int Money
        {
            get => _money;
            private set
            {
                if (SetProperty(ref _money, value))
                {
                    OnPropertyChanged(nameof(IsOutOfMoney));
                    RaiseCommandStates();
                }
            }
        }

        public int Bet
        {
            get => _bet;
            private set
            {
                if (SetProperty(ref _bet, value))
                    RaiseCommandStates();
            }
        }

        public bool IsRoundActive
        {
            get => _isRoundActive;
            private set
            {
                if (SetProperty(ref _isRoundActive, value))
                {
                    OnPropertyChanged(nameof(IsBetting));
                    RaiseCommandStates();
                }
            }
        }

        public bool IsResultVisible
        {
            get => _isResultVisible;
            private set
            {
                if (SetProperty(ref _isResultVisible, value))
                {
                    OnPropertyChanged(nameof(IsBetting));
                    OnPropertyChanged(nameof(ShowBetLabel));
                    RaiseCommandStates();
                }
            }
        }

        public bool IsAnimating
        {
            get => _isAnimating;
            private set
            {
                if (SetProperty(ref _isAnimating, value))
                {
                    OnPropertyChanged(nameof(IsBetting));
                    RaiseCommandStates();
                }
            }
        }

        public bool IsBetting => !IsRoundActive && !IsAnimating && !IsResultVisible;

        public bool ShowBetLabel => !IsResultVisible;

        public string ResultMessage
        {
            get => _resultMessage;
            private set => SetProperty(ref _resultMessage, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetProperty(ref _statusMessage, value);
        }

        public string PlayerTotalText
        {
            get => _playerTotalText;
            private set => SetProperty(ref _playerTotalText, value);
        }

        public string DealerTotalText
        {
            get => _dealerTotalText;
            private set => SetProperty(ref _dealerTotalText, value);
        }

        public bool CanChangeBet => IsBetting;

        public bool CanDeal => IsBetting && Money >= BetStep && Bet <= Money;

        public bool CanHit => IsRoundActive && !IsAnimating;

        public bool CanStand => IsRoundActive && !IsAnimating;

        public bool IsOutOfMoney => Money < BetStep;

        private void ChangeBet(int delta)
        {
            int max = Math.Max(BetStep, Money);
            Bet = Math.Clamp(Bet + delta, BetStep, max);
        }

        private void Deal()
        {
            if (!CanDeal)
                return;

            BuildShuffledDeck();
            PlayerHand.Clear();
            DealerHand.Clear();

            PlayerHand.Add(Draw());
            DealerHand.Add(Draw());
            PlayerHand.Add(Draw());

            var holeCard = Draw();
            holeCard.IsFaceDown = true;
            DealerHand.Add(holeCard);

            ResultMessage = string.Empty;
            IsRoundActive = true;
            StatusMessage = "Hit or stand?";
            RefreshTotals();
        }

        private async Task HitAsync()
        {
            if (!CanHit)
                return;

            IsAnimating = true;
            try
            {
                var card = Draw();
                card.IsFaceDown = true;
                PlayerHand.Add(card);

                await Task.Delay(DrawDelayMs);
                await ShakeAsync(dealer: false);

                card.IsFaceDown = false;
                RefreshTotals();

                if (HandValue(PlayerHand) > BlackjackTarget)
                {
                    IsRoundActive = false;
                    await Task.Delay(SettleDelayMs);
                    Settle();
                }
            }
            finally
            {
                IsAnimating = false;
            }
        }

        private async Task StandAsync()
        {
            if (!CanStand)
                return;

            IsAnimating = true;
            try
            {
                await FinishRoundAsync();
            }
            finally
            {
                IsAnimating = false;
            }
        }

        /// <summary>
        /// Reveals the dealer's hole card, lets the dealer draw (unless the player busted),
        /// then settles the bet after a short pause.
        /// </summary>
        private async Task FinishRoundAsync()
        {
            IsRoundActive = false;

            await Task.Delay(DrawDelayMs);
            await ShakeAsync(dealer: true);
            DealerHand[1].IsFaceDown = false;
            RefreshTotals();

            while (HandValue(DealerHand) < DealerStandValue)
            {
                var card = Draw();
                card.IsFaceDown = true;
                DealerHand.Add(card);

                await Task.Delay(DrawDelayMs);
                await ShakeAsync(dealer: true);

                card.IsFaceDown = false;
                RefreshTotals();
            }

            await Task.Delay(SettleDelayMs);
            Settle();
        }

        private Task ShakeAsync(bool dealer)
        {
            return ShakeHandAsync is null ? Task.CompletedTask : ShakeHandAsync(dealer);
        }

        private void Settle()
        {
            RefreshTotals();

            int playerValue = HandValue(PlayerHand);
            int dealerValue = HandValue(DealerHand);

            string result;
            if (playerValue > BlackjackTarget)
                result = ResultDealerWin;
            else if (dealerValue > BlackjackTarget || playerValue > dealerValue)
                result = ResultPlayerWin;
            else if (dealerValue > playerValue)
                result = ResultDealerWin;
            else
                result = ResultDealerAdvantage;

            int change = result == ResultPlayerWin ? Bet : -Bet;
            Money += change;
            _ = ShowMoneyChangeAsync?.Invoke(change);
            StatusMessage = change > 0 ? $"You won {Bet}." : $"You lost {Bet}.";

            if (IsOutOfMoney)
                StatusMessage = "You are out of money. Press Reset to start again.";

            Bet = Math.Clamp(Bet, BetStep, Math.Max(BetStep, Money));

            ResultMessage = result;
            IsResultVisible = true;
        }

        private void Reset()
        {
            Money = StartingMoney;
            Bet = BetStep;
            ResultMessage = string.Empty;
            IsResultVisible = false;
            StatusMessage = PromptBet;
            ShowPlaceholderHands();
        }

        private void DismissResult()
        {
            IsResultVisible = false;
            ShowPlaceholderHands();

            if (!IsOutOfMoney)
                StatusMessage = PromptBet;
        }

        /// <summary>
        /// Shows two face-down cards for each side while no round is in play.
        /// </summary>
        private void ShowPlaceholderHands()
        {
            PlayerHand.Clear();
            DealerHand.Clear();

            for (int i = 0; i < 2; i++)
            {
                PlayerHand.Add(new Card(1, CardSuit.Spades) { IsFaceDown = true });
                DealerHand.Add(new Card(1, CardSuit.Spades) { IsFaceDown = true });
            }

            RefreshTotals();
        }

        private void RefreshTotals()
        {
            PlayerTotalText = $"Player: {HandValue(PlayerHand)}";
            DealerTotalText = DealerHand.Any(c => c.IsFaceDown)
                ? "Dealer: ?"
                : $"Dealer: {HandValue(DealerHand)}";
        }

        private void RaiseCommandStates()
        {
            IncreaseBetCommand.ChangeCanExecute();
            DecreaseBetCommand.ChangeCanExecute();
            DealCommand.ChangeCanExecute();
            HitCommand.ChangeCanExecute();
            StandCommand.ChangeCanExecute();
            ResetCommand.ChangeCanExecute();
            OnPropertyChanged(nameof(CanChangeBet));
            OnPropertyChanged(nameof(CanDeal));
            OnPropertyChanged(nameof(CanHit));
            OnPropertyChanged(nameof(CanStand));
            OnPropertyChanged(nameof(IsBetting));
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

        private Card Draw()
        {
            var card = _deck[^1];
            _deck.RemoveAt(_deck.Count - 1);
            return card;
        }

        /// <summary>
        /// Best hand total: aces count as 11 unless that would bust.
        /// </summary>
        private static int HandValue(IEnumerable<Card> cards)
        {
            int total = 0;
            int aces = 0;

            foreach (var card in cards)
            {
                if (card.IsFaceDown)
                    continue;

                total += card.BaseValue;
                if (card.Rank == 1)
                    aces++;
            }

            while (aces > 0 && total + 10 <= BlackjackTarget)
            {
                total += 10;
                aces--;
            }

            return total;
        }
    }
}
