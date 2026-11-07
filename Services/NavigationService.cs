using PartyGames.Features.Blackjack;
using PartyGames.Features.CoinFlip;
using PartyGames.Features.DiceRoller;
using PartyGames.Features.HigherLower;
using PartyGames.Features.TicTacToe;


namespace PartyGames.Services
{
    /// <summary>
    /// Service for handling application navigation
    /// </summary>
    public interface INavigationService
    {
        Task NavigateToGameAsync(string gameType);
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToGameAsync(string gameType)
        {
            Page targetPage = gameType switch
            {
                "dice" => new DiceGamePage(_serviceProvider.GetRequiredService<DiceGameViewModel>()),
                "coinflip" => new CoinFlipPage(_serviceProvider.GetRequiredService<CoinFlipViewModel>()),
                "blackjack" => new BlackjackPage(_serviceProvider.GetRequiredService<BlackjackViewModel>()),
                "tictactoe" => new TicTacToePage(_serviceProvider.GetRequiredService<TicTacToeViewModel>()),
                "higherlower" => new HigherLowerPage(_serviceProvider.GetRequiredService<HigherLowerViewModel>()),
                "higherlowerplus" => new HigherLowerPage(_serviceProvider.GetRequiredService<HigherLowerPlusViewModel>()),
                _ => throw new InvalidOperationException($"Unknown game type: {gameType}")
            };

            await Application.Current!.Windows[0].Page!.Navigation.PushAsync(targetPage);
        }
    }
}



