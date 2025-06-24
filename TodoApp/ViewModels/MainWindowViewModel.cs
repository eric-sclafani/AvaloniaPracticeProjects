using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TodoApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	public ObservableCollection<TodoItemViewModel> ToDoItems { get; } = [];
	
	[ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
	private string? _newItemContent;
	
	[RelayCommand (CanExecute = nameof(CanAddItem))]
	private void AddItem()
	{
		ToDoItems.Add(new TodoItemViewModel() {Content = NewItemContent});
		NewItemContent = null;
	}

	[RelayCommand]
	private void RemoveItem(TodoItemViewModel item)
	{
		ToDoItems.Remove(item);
	}
	
	private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemContent);
}