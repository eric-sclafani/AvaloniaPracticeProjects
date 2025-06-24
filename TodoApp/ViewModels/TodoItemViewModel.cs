using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using TodoApp.Models;

namespace TodoApp.ViewModels;

public partial class TodoItemViewModel : ViewModelBase
{
	[ObservableProperty] private bool _isChecked;

	[ObservableProperty] private string? _content;
	
	public TodoItemViewModel(){}

	public TodoItemViewModel(TodoItem item)
	{
		IsChecked = item.IsChecked;
		Content = item.Content;
	}

	public TodoItem GetTodoItem()
	{
		return new TodoItem()
		{
			IsChecked = IsChecked,
			Content = Content
		};
	}
	
	
	
}