using PartyGames.ViewModels;

namespace PartyGames.Features.Blackjack
{
    public enum CardSuit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    /// <summary>
    /// A single playing card. Rank 1 = Ace, 11 = Jack, 12 = Queen, 13 = King.
    /// </summary>
    public class Card : BaseViewModel
    {
        private bool _isFaceDown;

        public Card(int rank, CardSuit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public int Rank { get; }

        public CardSuit Suit { get; }

        public bool IsFaceDown
        {
            get => _isFaceDown;
            set => SetProperty(ref _isFaceDown, value);
        }

        /// <summary>
        /// Blackjack value before ace adjustment (aces count as 1).
        /// </summary>
        public int BaseValue => Rank >= 10 ? 10 : Rank;

        public string RankText => Rank switch
        {
            1 => "A",
            11 => "J",
            12 => "Q",
            13 => "K",
            _ => Rank.ToString()
        };

        public string SuitSymbol => Suit switch
        {
            CardSuit.Hearts => "♥",
            CardSuit.Diamonds => "♦",
            CardSuit.Clubs => "♣",
            _ => "♠"
        };

        public Color SuitColor => Suit is CardSuit.Hearts or CardSuit.Diamonds
            ? Color.FromArgb("#C62828")
            : Color.FromArgb("#111111");
    }
}
