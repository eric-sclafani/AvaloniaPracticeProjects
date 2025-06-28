
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using MusicStore.Messages;

namespace MusicStore.Views;

public partial class MusicStoreWindow : Window
{
	public MusicStoreWindow()
	{
		InitializeComponent();
		
		// Register a handler to listen for the message sent by the view model
		WeakReferenceMessenger.Default.Register<MusicStoreWindow, MusicStoreClosedMessage>(this,
			static (window, message) =>
			{
				window.Close(message.SelectedAlbum); // close the dialog and return the selected album
			});
	}
}