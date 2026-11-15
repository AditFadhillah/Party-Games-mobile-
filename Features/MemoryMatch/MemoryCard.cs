using PartyGames.ViewModels;

namespace PartyGames.Features.MemoryMatch
{
    /// <summary>
    /// One face-down tile in Memory Match.
    /// </summary>
    public class MemoryCard : BaseViewModel
    {
        private static readonly Brush BackBrush = new SolidColorBrush(Color.FromArgb("#FF9500"));
        private static readonly Brush FaceBrush = new SolidColorBrush(Colors.White);
        private static readonly Brush MatchedBrush = new SolidColorBrush(Color.FromArgb("#C8E6C9"));

        private bool _isFaceUp;
        private bool _isMatched;

        public MemoryCard(int index, string emoji)
        {
            Index = index;
            Emoji = emoji;
        }

        public int Index { get; }

        public string Emoji { get; }

        public bool IsFaceUp
        {
            get => _isFaceUp;
            set
            {
                if (SetProperty(ref _isFaceUp, value))
                    Refresh();
            }
        }

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                if (SetProperty(ref _isMatched, value))
                    Refresh();
            }
        }

        public string Symbol => IsFaceUp || IsMatched ? Emoji : "?";

        public Brush CardBrush => IsMatched ? MatchedBrush : IsFaceUp ? FaceBrush : BackBrush;

        private void Refresh()
        {
            OnPropertyChanged(nameof(Symbol));
            OnPropertyChanged(nameof(CardBrush));
        }
    }
}
