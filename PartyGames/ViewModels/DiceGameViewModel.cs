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
        private const int MinDice = 1;
        private const int MaxDice = 6;
        private int _diceCount = 2;
        private int _total = 0;
        private int _bestRoll = 0;

        public int DiceCount
        {
            get => _diceCount;
            set => SetProperty(ref _diceCount, value);
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
        public ObservableCollection<int> Dice { get; }
        public ICommand RollCommand { get; }
        public ICommand IncreaseDiceCommand { get; }
        public ICommand DecreaseDiceCommand { get; }

        public DiceGameViewModel(IGameService gameService, IScoreService scoreService)
        {
            _gameService = gameService;
            _scoreService = scoreService;

            Title = "Dice Roller";
            RollHistory = new ObservableCollection<int>();
            Dice = new ObservableCollection<int>();
            ResetDice();
            RollCommand = new Command(OnRoll);
            IncreaseDiceCommand = new Command(() => ChangeDiceCount(1));
            DecreaseDiceCommand = new Command(() => ChangeDiceCount(-1));
        }

        private void ChangeDiceCount(int delta)
        {
            if (IsBusy)
                return;

            int newCount = Math.Clamp(DiceCount + delta, MinDice, MaxDice);
            if (newCount == DiceCount)
                return;

            DiceCount = newCount;
            ResetDice();
        }

        private void ResetDice()
        {
            Dice.Clear();
            for (int i = 0; i < DiceCount; i++)
                Dice.Add(0);

            Total = 0;
        }

        private async void OnRoll()
        {
            IsBusy = true;

            try
            {
                // Simulate dice roll animation delay
                await Task.Delay(500);

                // Roll the dice
                int[] values = _gameService.RollDice(DiceCount);
                for (int i = 0; i < values.Length; i++)
                    Dice[i] = values[i];
                Total = values.Sum();
                


                // Track best roll
                if (Total > BestRoll)
                {
                    BestRoll = Total;
                }

                // Add to history
                RollHistory.Insert(0, Total);
                
                // Keep only last 10 rolls
                if (RollHistory.Count > 10)
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
