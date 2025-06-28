using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MusicStore.Messages;
using MusicStore.Models;

namespace MusicStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	public ObservableCollection<AlbumViewModel> Albums { get; } = [];
	public MainWindowViewModel()
	{
		LoadAlbums();
	}

	[RelayCommand]
	private async Task AddAlbumAsync()
	{
		var album = await WeakReferenceMessenger.Default.Send(new PurchaseAlbumMessage());
		if (album is not null)
		{
			Albums.Add(album);
			await album.SaveToDiskAsync(); // Add this line
		}
	}
	
	private async void LoadAlbums()
	{
		var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x)).ToList();
		foreach (var album in albums)
		{
			Albums.Add(album);
		}
		var coverTasks = albums.Select(album => album.LoadCover());
		await Task.WhenAll(coverTasks);
	}
}