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
        private const int MaxHands = 4;
        private const int CharlieCardCount = 5;
        private const int DrawDelayMs = 400;
        private const int SettleDelayMs = 800;
        private const string ResultPlayerWin = "Player win";
        private const string ResultDealerWin = "Dealer win";
        private const string ResultPlayerBlackjack = "Blackjack!";
        private const string ResultPush = "Push";
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
        private string _dealerTotalText = "Dealer: ?";

        public BlackjackViewModel()
        {
            Title = "Blackjack";

            IncreaseBetCommand = new Command(() => ChangeBet(BetStep), () => CanChangeBet && Bet + BetStep <= Money);
            DecreaseBetCommand = new Command(() => ChangeBet(-BetStep), () => CanChangeBet && Bet > BetStep);
            DealCommand = new Command(async () => await DealAsync(), () => CanDeal);
            HitCommand = new Command(async () => await HitAsync(), () => CanHit);
            StandCommand = new Command(async () => await StandAsync(), () => CanStand);
            DoubleDownCommand = new Command(async () => await DoubleDownAsync(), () => CanDoubleDown);
            SplitCommand = new Command(async () => await SplitAsync(), () => CanSplit);
            ResetCommand = new Command(Reset, () => IsOutOfMoney);
            DismissResultCommand = new Command(DismissResult);

            ShowPlaceholderHands();
        }

        public ObservableCollection<BlackjackHand> Hands { get; } = new();

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
        public Command DoubleDownCommand { get; }
        public Command SplitCommand { get; }
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

        public string DealerTotalText
        {
            get => _dealerTotalText;
            private set => SetProperty(ref _dealerTotalText, value);
        }

        public bool CanChangeBet => IsBetting;

        public bool CanDeal => IsBetting && Money >= BetStep && Bet <= Money;

        /// <summary>
        /// Money not yet committed to a hand in the current round.
        /// </summary>
        public int AvailableFunds => Money - Hands.Sum(h => h.Stake);

        public bool CanHit => IsRoundActive && !IsAnimating && Active is not null;

        public bool CanStand => IsRoundActive && !IsAnimating && Active is not null;

        public bool CanDoubleDown => IsRoundActive && !IsAnimating
            && Active is { } hand && hand.Cards.Count == 2 && AvailableFunds >= hand.Stake;

        public bool CanSplit => IsRoundActive && !IsAnimating
            && Active is { IsSplitAce: false } hand
            && hand.Cards.Count == 2
            && hand.Cards[0].BaseValue == hand.Cards[1].BaseValue
            && Hands.Count < MaxHands
            && AvailableFunds >= hand.Stake;

        /// <summary>
        /// The hand the player is currently playing, or null when no hand is left to act on.
        /// </summary>
        private BlackjackHand? Active => Hands.FirstOrDefault(h => !h.IsDone);

        public bool IsOutOfMoney => Money < BetStep;

        private void ChangeBet(int delta)
        {
            int max = Math.Max(BetStep, Money);
            Bet = Math.Clamp(Bet + delta, BetStep, max);
        }

        private async Task DealAsync()
        {
            if (!CanDeal)
                return;

            BuildShuffledDeck();
            Hands.Clear();
            DealerHand.Clear();

            var firstHand = new BlackjackHand { Stake = Bet };
            firstHand.Cards.Add(Draw());
            DealerHand.Add(Draw());
            firstHand.Cards.Add(Draw());
            Hands.Add(firstHand);

            var holeCard = Draw();
            holeCard.IsFaceDown = true;
            DealerHand.Add(holeCard);

            ResultMessage = string.Empty;
            IsRoundActive = true;
            RefreshTotals();

            if (IsNatural(firstHand.Cards))
            {
                IsAnimating = true;
                try
                {
                    await FinishRoundAsync();
                }
                finally
                {
                    IsAnimating = false;
                }
                return;
            }

            StatusMessage = "Hit or stand?";
        }

        private async Task HitAsync()
        {
            if (!CanHit)
                return;

            IsAnimating = true;
            try
            {
                var hand = Active!;
                await DrawToHandAsync(hand);

                if (HandValue(hand.Cards) > BlackjackTarget || hand.Cards.Count >= CharlieCardCount)
                    hand.IsDone = true;

                await AdvanceRoundAsync();
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
                Active!.IsDone = true;
                await AdvanceRoundAsync();
            }
            finally
            {
                IsAnimating = false;
            }
        }

        private async Task DoubleDownAsync()
        {
            if (!CanDoubleDown)
                return;

            IsAnimating = true;
            try
            {
                var hand = Active!;
                hand.Stake *= 2;
                await DrawToHandAsync(hand);

                hand.IsDone = true;
                await AdvanceRoundAsync();
            }
            finally
            {
                IsAnimating = false;
            }
        }

        /// <summary>
        /// Splits a pair into two hands. Each hand gets a new card and a stake equal to the original bet.
        /// </summary>
        private async Task SplitAsync()
        {
            if (!CanSplit)
                return;

            IsAnimating = true;
            try
            {
                var hand = Active!;
                bool splitAces = hand.Cards[0].Rank == 1;

                var moved = hand.Cards[1];
                hand.Cards.RemoveAt(1);

                var newHand = new BlackjackHand { Stake = hand.Stake, IsSplitAce = splitAces };
                newHand.Cards.Add(moved);
                hand.IsSplitAce = splitAces;
                Hands.Insert(Hands.IndexOf(hand) + 1, newHand);
                RefreshTotals();

                await DrawToHandAsync(hand);
                await DrawToHandAsync(newHand);

                // Split aces receive one card each and then stand.
                if (splitAces)
                {
                    hand.IsDone = true;
                    newHand.IsDone = true;
                }

                await AdvanceRoundAsync();
            }
            finally
            {
                IsAnimating = false;
            }
        }

        /// <summary>
        /// Deals one face-up card to a player hand with a short animation.
        /// </summary>
        private async Task DrawToHandAsync(BlackjackHand hand)
        {
            var card = Draw();
            card.IsFaceDown = true;
            hand.Cards.Add(card);

            await Task.Delay(DrawDelayMs);
            await ShakeAsync(dealer: false);

            card.IsFaceDown = false;
            RefreshTotals();
        }

        /// <summary>
        /// Prompts for the next player hand, or ends the round once every hand is finished.
        /// </summary>
        private async Task AdvanceRoundAsync()
        {
            if (Active is { } next)
            {
                StatusMessage = Hands.Count == 1
                    ? "Hit or stand?"
                    : $"Hand {Hands.IndexOf(next) + 1}: hit, stand, double or split?";
                return;
            }

            // Every hand busted, so the dealer does not need to play.
            if (Hands.All(h => HandValue(h.Cards) > BlackjackTarget))
            {
                IsRoundActive = false;
                await Task.Delay(SettleDelayMs);
                Settle();
                return;
            }

            await FinishRoundAsync();
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

            int dealerValue = HandValue(DealerHand);
            bool dealerNatural = IsNatural(DealerHand);
            bool singleHand = Hands.Count == 1;

            var results = new List<string>();
            int net = 0;
            bool blackjack = false;

            foreach (var hand in Hands)
            {
                int playerValue = HandValue(hand.Cards);
                // A natural only counts on an unsplit opening hand.
                bool playerNatural = singleHand && IsNatural(hand.Cards);
                string result;
                int change;

                if (playerValue > BlackjackTarget)
                {
                    result = ResultDealerWin;
                    change = -hand.Stake;
                }
                else if (playerNatural && dealerNatural)
                {
                    result = ResultPush;
                    change = 0;
                }
                else if (dealerNatural)
                {
                    result = ResultDealerWin;
                    change = -hand.Stake;
                }
                else if (playerNatural)
                {
                    result = ResultPlayerBlackjack;
                    change = hand.Stake * 3 / 2;
                    blackjack = true;
                }
                else if (dealerValue > BlackjackTarget || playerValue > dealerValue)
                {
                    result = ResultPlayerWin;
                    change = hand.Stake;
                }
                else if (playerValue == dealerValue)
                {
                    result = ResultPush;
                    change = 0;
                }
                else
                {
                    result = ResultDealerWin;
                    change = -hand.Stake;
                }

                results.Add(result);
                net += change;
            }

            ResultMessage = string.Join("\n",
                results.Select((r, i) => singleHand ? r : $"Hand {i + 1}: {r}"));

            Money += net;
            if (net != 0)
                _ = ShowMoneyChangeAsync?.Invoke(net);

            StatusMessage = net switch
            {
                0 => results.All(r => r == ResultPush)
                    ? "Push. Your bet is returned."
                    : "Even round. No money won or lost.",
                < 0 => $"You lost {-net}.",
                _ => blackjack ? $"Blackjack! You won {net}." : $"You won {net}.",
            };

            if (IsOutOfMoney)
                StatusMessage = "You are out of money. Press Reset to start again.";

            Bet = Math.Clamp(Bet, BetStep, Math.Max(BetStep, Money));

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
            Hands.Clear();
            DealerHand.Clear();

            // Zero stake marks this as a placeholder rather than a real hand.
            var placeholder = new BlackjackHand();
            for (int i = 0; i < 2; i++)
            {
                placeholder.Cards.Add(new Card(1, CardSuit.Spades) { IsFaceDown = true });
                DealerHand.Add(new Card(1, CardSuit.Spades) { IsFaceDown = true });
            }
            Hands.Add(placeholder);

            RefreshTotals();
        }

        private void RefreshTotals()
        {
            foreach (var hand in Hands)
            {
                hand.Summary = hand.Stake == 0
                    ? string.Empty
                    : $"Total {HandValue(hand.Cards)}";
                hand.BetText = hand.Stake == 0
                    ? string.Empty
                    : $"Bet {hand.Stake}";
            }

            DealerTotalText = DealerHand.Any(c => c.IsFaceDown)
                ? "Dealer: ?"
                : $"Dealer: {HandValue(DealerHand)}";

            RaiseCommandStates();
        }

        private void RaiseCommandStates()
        {
            IncreaseBetCommand.ChangeCanExecute();
            DecreaseBetCommand.ChangeCanExecute();
            DealCommand.ChangeCanExecute();
            HitCommand.ChangeCanExecute();
            StandCommand.ChangeCanExecute();
            DoubleDownCommand.ChangeCanExecute();
            SplitCommand.ChangeCanExecute();
            ResetCommand.ChangeCanExecute();
            OnPropertyChanged(nameof(CanChangeBet));
            OnPropertyChanged(nameof(CanDeal));
            OnPropertyChanged(nameof(CanHit));
            OnPropertyChanged(nameof(CanStand));
            OnPropertyChanged(nameof(CanDoubleDown));
            OnPropertyChanged(nameof(CanSplit));
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

        private static bool IsNatural(IEnumerable<Card> cards)
        {
            var list = cards.ToList();
            return list.Count == 2 && HandValue(list) == BlackjackTarget;
        }

        /// <summary>
        /// Best hand total: aces count as 11 unless that would bust. Five or more non-busted cards count as 21.
        /// </summary>
        private static int HandValue(IEnumerable<Card> cards)
        {
            int total = 0;
            int aces = 0;
            int visibleCards = 0;

            foreach (var card in cards)
            {
                if (card.IsFaceDown)
                    continue;

                visibleCards++;
                total += card.BaseValue;
                if (card.Rank == 1)
                    aces++;
            }

            while (aces > 0 && total + 10 <= BlackjackTarget)
            {
                total += 10;
                aces--;
            }

            if (visibleCards >= CharlieCardCount && total <= BlackjackTarget)
                return BlackjackTarget;

            return total;
        }
    }
}
