using System.Collections.ObjectModel;
using System.Windows.Input;
using PartyGames.Services;
using PartyGames.ViewModels;

namespace PartyGames.Features.DiceRoller
{
    /// <summary>
    /// ViewModel for the Dice Game
    /// </summary>
    public class DiceGameViewModel : BaseViewModel
    {
        private readonly IGameService _gameService;
        private const int MinDice = 1;
        private const int MaxDice = 8;
        private static readonly int[] SupportedDieSides = { 4, 6, 8, 10, 12, 20 };
        private int _diceCount = 2;
        private int _dieSides = 6;
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

        public int DieSides
        {
            get => _dieSides;
            private set => SetProperty(ref _dieSides, value);
        }

        public ObservableCollection<int> RollHistory { get; }
        public ObservableCollection<DieResult> Dice { get; }
        public ObservableCollection<DieTypeOption> DieTypes { get; }
        public ICommand RollCommand { get; }
        public ICommand IncreaseDiceCommand { get; }
        public ICommand DecreaseDiceCommand { get; }

        public DiceGameViewModel(IGameService gameService)
        {
            _gameService = gameService;

            Title = "Dice Roller";
            RollHistory = new ObservableCollection<int>();
            Dice = new ObservableCollection<DieResult>();
            DieTypes = new ObservableCollection<DieTypeOption>();
            foreach (int sides in SupportedDieSides)
                DieTypes.Add(new DieTypeOption(sides, new Command(() => SelectDieType(sides))));
            UpdateDieTypeSelection();
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

        private void SelectDieType(int sides)
        {
            if (IsBusy || sides == DieSides)
                return;

            DieSides = sides;
            UpdateDieTypeSelection();

            // Best roll and history are only meaningful for a single die type
            BestRoll = 0;
            RollHistory.Clear();
            ResetDice();
        }

        private void UpdateDieTypeSelection()
        {
            foreach (DieTypeOption option in DieTypes)
                option.IsSelected = option.Sides == DieSides;
        }

        private void ResetDice()
        {
            Dice.Clear();
            for (int i = 0; i < DiceCount; i++)
                Dice.Add(new DieResult(0, DieSides));

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
                int[] values = _gameService.RollDice(DiceCount, DieSides);
                Dice.Clear();
                foreach (int value in values)
                    Dice.Add(new DieResult(value, DieSides));
                Total = values.Sum();
                


                // Track best roll
                if (Total > BestRoll)
                {
                    BestRoll = Total;
                }

                // Add to history
                RollHistory.Insert(0, Total);
                
                // Keep only last 30 rolls
                if (RollHistory.Count > 30)
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
