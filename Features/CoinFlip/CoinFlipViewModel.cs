using System.Collections.ObjectModel;
using System.Windows.Input;
using PartyGames.Services;
using PartyGames.ViewModels;

namespace PartyGames.Features.CoinFlip
{
    /// <summary>
    /// ViewModel for the Coin Flip Game
    /// </summary>
    public class CoinFlipViewModel : BaseViewModel
    {
        private const string HeadsImage = "gold_coin_sword.png";
        private const string TailsImage = "gold_coin_skull.png";
        private const string SpinSwordImage = "gold_coin_spin_sword.png";
        private const string SpinSkullImage = "gold_coin_spin_skull.png";
        private const int SpinFrameCount = 12;
        private const int SpinFrameDelayMs = 80;
        private const int MaxHistoryCount = 30;
        private const int RotationStepDegrees = 90;
        private const int RotationStepsPerCycle = 4;

        private readonly IGameService _gameService;
        private string _coinImage = HeadsImage;
        private bool _isFlipping;
        private double _coinRotation;

        public string CoinImage
        {
            get => _coinImage;
            set => SetProperty(ref _coinImage, value);
        }

        public double CoinRotation
        {
            get => _coinRotation;
            set => SetProperty(ref _coinRotation, value);
        }

        public bool IsFlipping
        {
            get => _isFlipping;
            set => SetProperty(ref _isFlipping, value);
        }

        public ICommand FlipCommand { get; }

        public ObservableCollection<string> FlipHistory { get; } = new();

        public CoinFlipViewModel(IGameService gameService)
        {
            _gameService = gameService;

            Title = "Coin Flip";
            FlipCommand = new Command(async () => await FlipAsync());
        }

        public void ClearHistory() => FlipHistory.Clear();

        private async Task FlipAsync()
        {
            if (IsFlipping)
                return;

            IsFlipping = true;

            try
            {
                bool isHeads = _gameService.FlipCoin();

                for (int i = 0; i < SpinFrameCount; i++)
                {
                    bool isSwordFrame = i % 2 == 0;
                    CoinImage = isSwordFrame ? SpinSwordImage : SpinSkullImage;
                    CoinRotation = (i % RotationStepsPerCycle) * RotationStepDegrees;
                    await Task.Delay(SpinFrameDelayMs);
                }

                CoinRotation = 0;
                CoinImage = isHeads ? HeadsImage : TailsImage;

                FlipHistory.Insert(0, CoinImage);
                if (FlipHistory.Count > MaxHistoryCount)
                    FlipHistory.RemoveAt(FlipHistory.Count - 1);
            }
            finally
            {
                IsFlipping = false;
            }
        }
    }
}
