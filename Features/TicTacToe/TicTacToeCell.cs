using PartyGames.ViewModels;

namespace PartyGames.Features.TicTacToe
{
    /// <summary>
    /// One square on the Tic-Tac-Toe board.
    /// </summary>
    public class TicTacToeCell : BaseViewModel
    {
        private static readonly Brush NormalBrush = new SolidColorBrush(Colors.White);
        private static readonly Brush WinningBrush = new SolidColorBrush(Color.FromArgb("#FFE6CC"));
        private static readonly Color XColor = Color.FromArgb("#1565C0");
        private static readonly Color OColor = Color.FromArgb("#C62828");

        private string _mark = string.Empty;
        private bool _isWinning;

        public TicTacToeCell(int index)
        {
            Index = index;
        }

        public int Index { get; }

        public string Mark
        {
            get => _mark;
            set
            {
                if (SetProperty(ref _mark, value))
                    OnPropertyChanged(nameof(MarkColor));
            }
        }

        public Color MarkColor => Mark == "O" ? OColor : XColor;

        public bool IsWinning
        {
            get => _isWinning;
            set
            {
                if (SetProperty(ref _isWinning, value))
                    OnPropertyChanged(nameof(CellBrush));
            }
        }

        public Brush CellBrush => IsWinning ? WinningBrush : NormalBrush;

        public void Reset()
        {
            Mark = string.Empty;
            IsWinning = false;
        }
    }
}
