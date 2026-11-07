using PartyGames.ViewModels;

namespace PartyGames.Features.Main;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
		BindingContext = viewModel;
	}
}
