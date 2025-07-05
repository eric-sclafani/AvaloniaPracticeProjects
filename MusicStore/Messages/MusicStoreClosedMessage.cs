using MusicStore.ViewModels;

namespace MusicStore.Messages;

public class MusicStoreClosedMessage(AlbumViewModel  selectedAlbum)
{
	public AlbumViewModel SelectedAlbums { get; } = selectedAlbum;
}