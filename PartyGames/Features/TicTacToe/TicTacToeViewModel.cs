using System.Collections.ObjectModel;
using PartyGames.ViewModels;

namespace PartyGames.Features.TicTacToe
{
    /// <summary>
    /// ViewModel for Tic-Tac-Toe in CPU and two-player modes. X always starts.
    /// </summary>
    public class TicTacToeViewModel : BaseViewModel
    {
        private const char Empty = ' ';
        private const char XMark = 'X';
        private const char OMark = 'O';
        private const int CpuDelayMs = 500;

        private enum CpuDifficulty { Easy, Medium, Hard }

        private static readonly int[][] WinningLines =
        {
            new[] { 0, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 },
            new[] { 0, 3, 6 }, new[] { 1, 4, 7 }, new[] { 2, 5, 8 },
            new[] { 0, 4, 8 }, new[] { 2, 4, 6 }
        };

        private readonly char[] _board = new char[9];
        private readonly Random _random = new();
        private char _currentMark = XMark;
        private char _humanMark = XMark;
        private CpuDifficulty _difficulty = CpuDifficulty.Medium;
        private CpuDifficulty _pendingDifficulty = CpuDifficulty.Medium;
        private char _pendingMark = XMark;
        private bool _isCpuMode;
        private bool _isModeSelected;
        private bool _isRoundOver;
        private bool _isResultVisible;
        private bool _isCpuTurn;
        private int _roundId;
        private int _xWins;
        private int _oWins;
        private int _draws;
        private string _statusMessage = string.Empty;
        private string _resultMessage = string.Empty;
        private string _scoreText = string.Empty;

        public TicTacToeViewModel()
        {
            Title = "Tic-Tac-Toe";

            for (int i = 0; i < 9; i++)
                Cells.Add(new TicTacToeCell(i));

            StartCpuCommand = new Command(StartCpuGame);
            StartTwoPlayerCommand = new Command(() => StartMode(false));
            TapCellCommand = new Command<int>(async index => await TapCellAsync(index));
            NewRoundCommand = new Command(NewRound);
            ChangeModeCommand = new Command(ChangeMode);
        }

        public ObservableCollection<TicTacToeCell> Cells { get; } = new();

        public Command StartCpuCommand { get; }
        public Command StartTwoPlayerCommand { get; }
        public Command<int> TapCellCommand { get; }
        public Command NewRoundCommand { get; }
        public Command ChangeModeCommand { get; }

        public bool IsCpuMode
        {
            get => _isCpuMode;
            private set => SetProperty(ref _isCpuMode, value);
        }

        public bool IsModeSelected
        {
            get => _isModeSelected;
            private set => SetProperty(ref _isModeSelected, value);
        }

        public bool IsCpuTurn
        {
            get => _isCpuTurn;
            private set => SetProperty(ref _isCpuTurn, value);
        }

        public bool IsResultVisible
        {
            get => _isResultVisible;
            private set => SetProperty(ref _isResultVisible, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetProperty(ref _statusMessage, value);
        }

        public string ResultMessage
        {
            get => _resultMessage;
            private set => SetProperty(ref _resultMessage, value);
        }

        public string ScoreText
        {
            get => _scoreText;
            private set => SetProperty(ref _scoreText, value);
        }

        public bool IsCpuBannerVisible => IsCpuMode && IsModeSelected;

        public bool IsEasySelected
        {
            get => _pendingDifficulty == CpuDifficulty.Easy;
            set { if (value) SetPendingDifficulty(CpuDifficulty.Easy); }
        }

        public bool IsMediumSelected
        {
            get => _pendingDifficulty == CpuDifficulty.Medium;
            set { if (value) SetPendingDifficulty(CpuDifficulty.Medium); }
        }

        public bool IsHardSelected
        {
            get => _pendingDifficulty == CpuDifficulty.Hard;
            set { if (value) SetPendingDifficulty(CpuDifficulty.Hard); }
        }

        public bool IsXSelected
        {
            get => _pendingMark == XMark;
            set { if (value) SetPendingMark(XMark); }
        }

        public bool IsOSelected
        {
            get => _pendingMark == OMark;
            set { if (value) SetPendingMark(OMark); }
        }

        private void SetPendingDifficulty(CpuDifficulty difficulty)
        {
            _pendingDifficulty = difficulty;
            OnPropertyChanged(nameof(IsEasySelected));
            OnPropertyChanged(nameof(IsMediumSelected));
            OnPropertyChanged(nameof(IsHardSelected));
        }

        private void SetPendingMark(char mark)
        {
            _pendingMark = mark;
            OnPropertyChanged(nameof(IsXSelected));
            OnPropertyChanged(nameof(IsOSelected));
        }

        private void StartCpuGame()
        {
            _difficulty = _pendingDifficulty;
            StartMode(true, _pendingMark);
        }

        private void StartMode(bool cpu, char humanMark = XMark)
        {
            IsCpuMode = cpu;
            _humanMark = humanMark;
            _xWins = 0;
            _oWins = 0;
            _draws = 0;
            IsModeSelected = true;
            OnPropertyChanged(nameof(IsCpuBannerVisible));
            UpdateScore();
            NewRound();
        }

        private void ChangeMode()
        {
            _roundId++;
            IsCpuTurn = false;
            IsResultVisible = false;
            IsModeSelected = false;
            OnPropertyChanged(nameof(IsCpuBannerVisible));
        }

        private void NewRound()
        {
            _roundId++;
            Array.Fill(_board, Empty);
            foreach (var cell in Cells)
                cell.Reset();

            _currentMark = XMark;
            _isRoundOver = false;
            IsCpuTurn = false;
            IsResultVisible = false;
            ResultMessage = string.Empty;
            StatusMessage = IsCpuMode ? TurnText(_humanMark) : "X's turn";

            if (IsCpuMode && _humanMark == OMark)
                _ = CpuTurnAsync();
        }

        private static string TurnText(char mark) => $"Your turn ({mark})";

        private async Task TapCellAsync(int index)
        {
            if (_isRoundOver || IsCpuTurn || _board[index] != Empty)
                return;

            PlaceMark(index, _currentMark);
            if (CheckRoundEnd())
                return;

            _currentMark = Opponent(_currentMark);
            if (IsCpuMode)
                await CpuTurnAsync();
            else
                StatusMessage = $"{_currentMark}'s turn";
        }

        private async Task CpuTurnAsync()
        {
            int round = _roundId;
            char cpuMark = Opponent(_humanMark);
            IsCpuTurn = true;
            StatusMessage = "CPU is thinking...";

            await Task.Delay(CpuDelayMs);
            if (round != _roundId)
                return;

            PlaceMark(ChooseCpuMove(cpuMark), cpuMark);
            IsCpuTurn = false;
            if (CheckRoundEnd())
                return;

            _currentMark = _humanMark;
            StatusMessage = TurnText(_humanMark);
        }

        private void PlaceMark(int index, char mark)
        {
            _board[index] = mark;
            Cells[index].Mark = mark.ToString();
        }

        private bool CheckRoundEnd()
        {
            var line = FindWinningLine(_board);
            if (line != null)
            {
                char winner = _board[line[0]];
                foreach (int i in line)
                    Cells[i].IsWinning = true;

                if (winner == XMark) _xWins++; else _oWins++;
                EndRound(WinnerMessage(winner));
                return true;
            }

            if (Array.IndexOf(_board, Empty) < 0)
            {
                _draws++;
                EndRound("It's a draw!");
                return true;
            }

            return false;
        }

        private string WinnerMessage(char winner)
        {
            if (!IsCpuMode)
                return $"{winner} wins!";

            return winner == _humanMark ? "You win!" : "CPU wins!";
        }

        private void EndRound(string message)
        {
            _isRoundOver = true;
            IsCpuTurn = false;
            ResultMessage = message;
            StatusMessage = message;
            IsResultVisible = true;
            UpdateScore();
        }

        private void UpdateScore()
        {
            ScoreText = $"X: {_xWins}   O: {_oWins}   Draws: {_draws}";
        }

        private static char Opponent(char mark) => mark == XMark ? OMark : XMark;

        private static int[]? FindWinningLine(char[] board)
        {
            foreach (var line in WinningLines)
            {
                char mark = board[line[0]];
                if (mark != Empty && mark == board[line[1]] && mark == board[line[2]])
                    return line;
            }

            return null;
        }

        private int ChooseCpuMove(char cpuMark)
        {
            bool random = _difficulty == CpuDifficulty.Easy
                || (_difficulty == CpuDifficulty.Medium && _random.Next(2) == 0);
            return random ? RandomMove() : BestMove(cpuMark);
        }

        private int RandomMove()
        {
            var empty = Enumerable.Range(0, _board.Length).Where(i => _board[i] == Empty).ToList();
            return empty[_random.Next(empty.Count)];
        }

        private int BestMove(char cpuMark)
        {
            var scores = new List<(int Index, int Score)>();
            for (int i = 0; i < _board.Length; i++)
            {
                if (_board[i] != Empty)
                    continue;

                _board[i] = cpuMark;
                scores.Add((i, Score(_board, _humanMark, 1, cpuMark)));
                _board[i] = Empty;
            }

            int best = scores.Max(s => s.Score);
            var bestMoves = scores.Where(s => s.Score == best).ToList();
            return bestMoves[_random.Next(bestMoves.Count)].Index;
        }

        /// <summary>
        /// Minimax score from the CPU's point of view. Faster wins and slower losses score higher.
        /// </summary>
        private static int Score(char[] board, char turn, int depth, char cpuMark)
        {
            var line = FindWinningLine(board);
            if (line != null)
                return board[line[0]] == cpuMark ? 10 - depth : depth - 10;

            if (Array.IndexOf(board, Empty) < 0)
                return 0;

            int best = turn == cpuMark ? int.MinValue : int.MaxValue;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != Empty)
                    continue;

                board[i] = turn;
                int score = Score(board, Opponent(turn), depth + 1, cpuMark);
                board[i] = Empty;

                best = turn == cpuMark ? Math.Max(best, score) : Math.Min(best, score);
            }

            return best;
        }
    }
}
