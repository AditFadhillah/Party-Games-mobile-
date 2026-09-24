using System.Collections.ObjectModel;
using System.Windows.Input;
using PartyGames.Services;

namespace PartyGames.ViewModels
{
    /// <summary>
    /// ViewModel for the Dice Game
    /// </summary>
    public class DiceGameViewModel : BaseViewModel
    {
        private readonly IGameService _gameService;
        private readonly IScoreService _scoreService;
        private int _die1 = 0;
        private int _die2 = 0;
        private int _total = 0;
        private int _bestRoll = 0;

        public int Die1
        {
            get => _die1;
            set => SetProperty(ref _die1, value);
        }

        public int Die2
        {
            get => _die2;
            set => SetProperty(ref _die2, value);
        }

        public int Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        public int BestRoll
        {
            get => _bestRoll;
            set => SetProperty(ref _bestRoll, value);
        }

        public ObservableCollection<int> RollHistory { get; }
        public ICommand RollCommand { get; }

        public DiceGameViewModel(IGameService gameService, IScoreService scoreService)
        {
            _gameService = gameService;
            _scoreService = scoreService;

            Title = "Dice Roller";
            RollHistory = new ObservableCollection<int>();
            RollCommand = new Command(OnRoll);
        }

        private async void OnRoll()
        {
            IsBusy = true;

            try
            {
                // Simulate dice roll animation delay
                await Task.Delay(500);

                // Roll the dice
                Total = _gameService.RollDice();
                
                // Extract individual dice (this is a simple simulation)
                Die1 = Total / 2 + (Total % 2 == 0 ? 0 : 1);
                Die2 = Total - Die1;
                if (Die2 < 1) Die2 = 1;
                if (Die1 > 6) Die1 = 6;
                if (Die2 > 6) Die2 = 6;

                // Track best roll
                if (Total > BestRoll)
                {
                    BestRoll = Total;
                }

                // Add to history
                RollHistory.Insert(0, Total);
                
                // Keep only last 5 rolls
                if (RollHistory.Count > 5)
                {
                    RollHistory.RemoveAt(RollHistory.Count - 1);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
