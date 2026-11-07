using System.Collections.ObjectModel;
using System.Windows.Input;
using PartyGames.Models;
using PartyGames.Services;
using PartyGames.ViewModels;

namespace PartyGames.Features.Main
{
    /// <summary>
    /// ViewModel for the main game selection screen
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private readonly IGameService _gameService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<Game> Games { get; }
        public ICommand SelectGameCommand { get; }

        public MainViewModel(IGameService gameService, INavigationService navigationService)
        {
            _gameService = gameService;
            _navigationService = navigationService;

            Title = "Party Games";
            Games = new ObservableCollection<Game>();
            SelectGameCommand = new Command<Game>(OnSelectGame);

            LoadGames();
        }

        private void LoadGames()
        {
            var games = _gameService.GetAllGames();
            foreach (var game in games)
            {
                Games.Add(game);
            }
        }

        private async void OnSelectGame(Game? game)
        {
            if (game == null)
                return;

            IsBusy = true;

            try
            {
                await _navigationService.NavigateToGameAsync(game.GameType);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
