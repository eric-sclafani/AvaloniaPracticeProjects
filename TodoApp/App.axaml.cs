using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using TodoApp.Services;
using TodoApp.ViewModels;
using TodoApp.Views;

namespace TodoApp;

public partial class App : Application
{
	
	private readonly MainWindowViewModel _mainViewModel = new ();
	
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}
	
	private bool _canClose; // This flag is used to check if window is allowed to close
	private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
	{
		e.Cancel = !_canClose; // cancel closing event first time

		if (!_canClose)
		{
			// To save the items, we map them to the ToDoItem-Model which is better suited for I/O operations
			var itemsToSave = _mainViewModel.ToDoItems.Select(item => item.GetTodoItem());
			await ToDoListFileService.SaveToFileAsync(itemsToSave);

			// Set _canClose to true and Close this Window again
			_canClose = true;
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.Shutdown();
			}
		}
	}

	public override async void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			DisableAvaloniaDataAnnotationValidation();
			desktop.MainWindow = new MainWindow
			{
				DataContext = new MainWindowViewModel(),
			};
			
			// Listen to the ShutdownRequested-event
			desktop.ShutdownRequested += DesktopOnShutdownRequested;
		}

		base.OnFrameworkInitializationCompleted();
		await InitMainViewModelAsync();
	}

	private void DisableAvaloniaDataAnnotationValidation()
	{
		// Get an array of plugins to remove
		var dataValidationPluginsToRemove =
			BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

		// remove each entry found
		foreach (var plugin in dataValidationPluginsToRemove)
		{
			BindingPlugins.DataValidators.Remove(plugin);
		}
	}
	
	private async Task InitMainViewModelAsync()
	{
		// get the items to load
		var itemsLoaded = await ToDoListFileService.LoadFromFileAsync();

		if (itemsLoaded is not null)
		{
			foreach (var item in itemsLoaded)
			{
				_mainViewModel.ToDoItems.Add(new TodoItemViewModel(item));
			}
		}
	}
}