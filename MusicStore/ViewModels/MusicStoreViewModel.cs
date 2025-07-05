using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MusicStore.Messages;
using MusicStore.Models;

namespace MusicStore.ViewModels;

public partial class MusicStoreViewModel : ViewModelBase
{
	[ObservableProperty] public partial string? SearchText { get; set; }
	[ObservableProperty] public partial bool IsBusy { get; private set; }
	[ObservableProperty] public partial AlbumViewModel? SelectedAlbum { get; set; }

	public ObservableCollection<AlbumViewModel> SearchResults { get; } = [];
	private CancellationTokenSource? _cancellationTokenSource;
	
	partial void OnSearchTextChanged(string? value)
	{
		_ = DoSearch(SearchText);
	}

	private async Task DoSearch(string? term)
	{
		_cancellationTokenSource?.Cancel();
		_cancellationTokenSource = new CancellationTokenSource();
		var cancellationToken = _cancellationTokenSource.Token;

		IsBusy = true;
		SearchResults.Clear();

		var albums = await Album.SearchAsync(term);

		foreach (var album in albums)
		{
			var vm = new AlbumViewModel(album);
			SearchResults.Add(vm);
		}

		if (!cancellationToken.IsCancellationRequested)
		{
			LoadCovers(cancellationToken);
		}

		IsBusy = false;
	}

	// Whenever search results are returned, load all album covers. Is also cancellable.
	private async void LoadCovers(CancellationToken cancellationToken)
	{
		foreach (var album in SearchResults.ToList())
		{
			await album.LoadCover();
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}
		}
	}

	[RelayCommand]
	private void BuyMusic()
	{
		if (SelectedAlbum is not null)
		{
			WeakReferenceMessenger.Default.Send(new MusicStoreClosedMessage(SelectedAlbum));
		}
	}
}