using MusicStore.Models;
using Avalonia.Media.Imaging;
using System.Threading.Tasks;
using Avalonia.Controls.Documents;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MusicStore.ViewModels;

public partial class AlbumViewModel : ViewModelBase
{
	private readonly Album _album;
	[ObservableProperty] public partial Bitmap? Cover { get; set; }

	public AlbumViewModel(Album album)
	{
		_album = album;
	}
	
	public string Artist => _album.Artist;
	
	public string Title => _album.Title;

	public async Task LoadCover()
	{
		await using var imageStream = await _album.LoadCoverBitmapAync();
		Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
	}
	
	public async Task SaveToDiskAsync()
	{
		await _album.SaveAsync();

		if (Cover != null)
		{
			var bitmap = Cover;

			await Task.Run(() =>
			{
				using var fs = _album.SaveCoverBitmapStream();
				bitmap.Save(fs);
			});
		}
	}
	
	
}