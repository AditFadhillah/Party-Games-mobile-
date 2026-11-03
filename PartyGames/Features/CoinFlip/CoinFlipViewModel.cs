using System.Windows.Input;
using PartyGames.Services;

namespace PartyGames.ViewModels
{
    /// <summary>
    /// ViewModel for the Coin Flip Game
    /// </summary>
    public class CoinFlipViewModel : BaseViewModel
    {
        private readonly IGameService _gameService;
        private readonly IScoreService _scoreService;
        private string _coinDisplay = "🪙";
        private int _wins = 0;
        private int _losses = 0;
        private int _streak = 0;
        private bool _isFlipping = false;
        private string _resultMessage = "Choose heads or tails";

        public string CoinDisplay
        {
            get => _coinDisplay;
            set => SetProperty(ref _coinDisplay, value);
        }

        public int Wins
        {
            get => _wins;
            set => SetProperty(ref _wins, value);
        }

        public int Losses
        {
            get => _losses;
            set => SetProperty(ref _losses, value);
        }

        public int Streak
        {
            get => _streak;
            set => SetProperty(ref _streak, value);
        }

        public bool IsFlipping
        {
            get => _isFlipping;
            set => SetProperty(ref _isFlipping, value);
        }

        public string ResultMessage
        {
            get => _resultMessage;
            set => SetProperty(ref _resultMessage, value);
        }

        public ICommand FlipHeadsCommand { get; }
        public ICommand FlipTailsCommand { get; }

        public CoinFlipViewModel(IGameService gameService, IScoreService scoreService)
        {
            _gameService = gameService;
            _scoreService = scoreService;

            Title = "Coin Flip";
            FlipHeadsCommand = new Command(async () => await OnFlip(true));
            FlipTailsCommand = new Command(async () => await OnFlip(false));
        }

        private async Task OnFlip(bool predictHeads)
        {
            if (IsFlipping)
                return;

            IsFlipping = true;
            IsBusy = true;

            try
            {
                // Animate coin flip
                for (int i = 0; i < 10; i++)
                {
                    CoinDisplay = i % 2 == 0 ? "🪙" : "🪙";
                    await Task.Delay(100);
                }

                // Get result
                bool isHeads = _gameService.FlipCoin();
                CoinDisplay = isHeads ? "Heads ✓" : "Tails ✓";

                // Check if prediction was correct
                bool won = (predictHeads && isHeads) || (!predictHeads && !isHeads);

                if (won)
                {
                    Wins++;
                    Streak++;
                    ResultMessage = "🎉 You Won!";
                }
                else
                {
                    Losses++;
                    Streak = 0;
                    ResultMessage = "😞 You Lost!";
                }

                // Reset after delay
                await Task.Delay(2000);
                CoinDisplay = "🪙";
                ResultMessage = "Choose heads or tails";
            }
            finally
            {
                IsFlipping = false;
                IsBusy = false;
            }
        }
    }
}
