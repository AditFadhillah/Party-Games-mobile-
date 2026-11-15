namespace PartyGames.Features.MemoryMatch;

public partial class MemoryMatchPage : ContentPage
{
	private readonly MemoryMatchViewModel _viewModel;

	public MemoryMatchPage(MemoryMatchViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		_viewModel.StopTimer();
	}

	// Squeeze the tapped card for feedback; the flip itself is handled by the command.
	private async void OnCardTapped(object? sender, TappedEventArgs e)
	{
		if (sender is not VisualElement card)
			return;

		await card.ScaleToAsync(0.9, 80);
		await card.ScaleToAsync(1, 80);
	}
}
