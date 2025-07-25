using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using MusicStore.Messages;
using MusicStore.ViewModels;

namespace MusicStore.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();

		if (!Design.IsDesignMode)
		{
			//  Whenever 'Send(new PurchaseAlbumMessage())' is called, invoke this callback on the MainWindow instance
			WeakReferenceMessenger.Default.Register<MainWindow, PurchaseAlbumMessage>(this, static (w, m) =>
			{
				// Create an instance of MusicStoreWindow and set MusicStoreViewModel as its DataContext
				var dialog = new MusicStoreWindow()
				{
					DataContext = new MusicStoreViewModel()
				};
				// Show dialog window and reply with returned AlbumViewModel or null when the dialog is closed.
				m.Reply(dialog.ShowDialog<AlbumViewModel?>(w)!);
			});
		}
	}
}