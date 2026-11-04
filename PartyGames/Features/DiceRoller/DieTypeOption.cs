using System.Windows.Input;
using PartyGames.ViewModels;

namespace PartyGames.Features.DiceRoller
{
    /// <summary>
    /// Selectable die type (for example d6) shown in the die type selector
    /// </summary>
    public class DieTypeOption : BaseViewModel
    {
        private bool _isSelected;

        public DieTypeOption(int sides, ICommand selectCommand)
        {
            Sides = sides;
            Name = $"d{sides}";
            SelectCommand = selectCommand;
        }

        public int Sides { get; }
        public string Name { get; }
        public ICommand SelectCommand { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
                OnPropertyChanged(nameof(BackgroundColor));
                OnPropertyChanged(nameof(TextColor));
            }
        }

        public Color BackgroundColor => IsSelected ? Color.FromArgb("#FF9500") : Color.FromArgb("#E8E8E8");
        public Color TextColor => IsSelected ? Colors.White : Color.FromArgb("#333333");
    }
}
