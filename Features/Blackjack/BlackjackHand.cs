using System.Collections.ObjectModel;
using PartyGames.ViewModels;

namespace PartyGames.Features.Blackjack
{
    /// <summary>
    /// One player hand. A round holds several of these after a split, each with its own stake.
    /// </summary>
    public class BlackjackHand : BaseViewModel
    {
        private int _stake;
        private bool _isDone;
        private bool _isSplitAce;
        private string _summary = string.Empty;
        private string _betText = string.Empty;

        public ObservableCollection<Card> Cards { get; } = new();

        /// <summary>
        /// The amount wagered on this hand. Zero for the placeholder hand shown between rounds.
        /// </summary>
        public int Stake
        {
            get => _stake;
            set => SetProperty(ref _stake, value);
        }

        /// <summary>
        /// True once the player has stood, doubled, busted, or the hand can take no more cards.
        /// </summary>
        public bool IsDone
        {
            get => _isDone;
            set => SetProperty(ref _isDone, value);
        }

        /// <summary>
        /// True for hands created by splitting aces. These receive one card and then stand.
        /// </summary>
        public bool IsSplitAce
        {
            get => _isSplitAce;
            set => SetProperty(ref _isSplitAce, value);
        }

        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        public string BetText
        {
            get => _betText;
            set => SetProperty(ref _betText, value);
        }
    }
}
