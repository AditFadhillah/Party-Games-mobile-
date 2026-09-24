using System.Collections.ObjectModel;
using System.Windows.Input;
using PartyGames.Models;
using PartyGames.Services;

namespace PartyGames.ViewModels
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

        private static void Log(string message)
        {
            Console.WriteLine(message);
            File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] {message}\n");
        }

        public MainViewModel(IGameService gameService, INavigationService navigationService)
        {
            Log("[MainViewModel.ctor] Constructor called");
            _gameService = gameService;
            _navigationService = navigationService;

            Title = "Party Games";
            Games = new ObservableCollection<Game>();
            SelectGameCommand = new Command<Game>(OnSelectGame);

            LoadGames();
            Log("[MainViewModel.ctor] Constructor completed");
        }

        private void LoadGames()
        {
            var games = _gameService.GetAllGames();
            foreach (var game in games)
            {
                Games.Add(game);
            }
        }

        private async void OnSelectGame(Game game)
        {
            Log($"[MainViewModel] OnSelectGame called with GameType: {game?.GameType}");
            
            if (game == null)
            {
                Log("[MainViewModel] Game object was null");
                return;
            }

            IsBusy = true;

            try
            {
                Log($"[MainViewModel] Calling NavigateToGameAsync with type: {game.GameType}");
                // Navigate to the appropriate game page based on game type
                await _navigationService.NavigateToGameAsync(game.GameType);
                Log($"[MainViewModel] Navigation completed successfully");
            }
            catch (Exception ex)
            {
                Log($"[MainViewModel] Navigation error: {ex.Message}");
                Log($"[MainViewModel] Stack trace: {ex.StackTrace}");
            }
            finally
            {
                IsBusy = false;
                Log("[MainViewModel] OnSelectGame finally block completed");
            }
        }
    }
}
