using PartyGames.Views;
using PartyGames.ViewModels;

namespace PartyGames.Services
{
    /// <summary>
    /// Service for handling application navigation
    /// </summary>
    public interface INavigationService
    {
        Task NavigateToGameAsync(string gameType);
        Task GoBackAsync();
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        private static void Log(string message)
        {
            Console.WriteLine(message);
            File.AppendAllText(@"C:\temp\partygames_debug.log", $"[{DateTime.Now:HH:mm:ss.fff}] {message}\n");
        }

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToGameAsync(string gameType)
        {
            try
            {
                Log($"[NavigationService] Starting navigation to: {gameType}");
                
                Page targetPage = null;
                
                try
                {
                    switch (gameType)
                    {
                        case "dice":
                            Log($"[NavigationService] Attempting to resolve DiceGameViewModel");
                            var diceVM = _serviceProvider.GetRequiredService<DiceGameViewModel>();
                            Log($"[NavigationService] DiceGameViewModel resolved successfully");
                            targetPage = new DiceGamePage(diceVM);
                            Log($"[NavigationService] DiceGamePage created");
                            break;
                            
                        case "coinflip":
                            Log($"[NavigationService] Attempting to resolve CoinFlipViewModel");
                            var coinVM = _serviceProvider.GetRequiredService<CoinFlipViewModel>();
                            Log($"[NavigationService] CoinFlipViewModel resolved successfully");
                            targetPage = new CoinFlipPage(coinVM);
                            Log($"[NavigationService] CoinFlipPage created");
                            break;
                            
                        case "blackjack":
                            Log($"[NavigationService] Creating BlackjackPage");
                            targetPage = new BlackjackPage();
                            Log($"[NavigationService] BlackjackPage created");
                            break;
                            
                        default:
                            throw new InvalidOperationException($"Unknown game type: {gameType}");
                    }
                }
                catch (Exception vmEx)
                {
                    Log($"[NavigationService] ERROR resolving ViewModel for {gameType}: {vmEx.Message}");
                    Log($"[NavigationService] Stack trace: {vmEx.StackTrace}");
                    throw;
                }

                if (targetPage == null)
                    throw new InvalidOperationException($"Failed to create page for game type: {gameType}");

                Log($"[NavigationService] Pushing page to navigation stack");
                await Application.Current!.MainPage!.Navigation.PushAsync(targetPage);
                Log($"[NavigationService] Navigation completed successfully");
            }
            catch (Exception ex)
            {
                Log($"[NavigationService] FATAL ERROR: {ex.Message}");
                Log($"[NavigationService] Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task GoBackAsync()
        {
            await Application.Current!.MainPage!.Navigation.PopAsync();
        }
    }
}



