using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotePad.Models;

namespace NotePad.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	public ObservableCollection<NoteViewModel> Notes { get; set; } = [];
	
	[ObservableProperty] private string? newNoteTitle;
	[ObservableProperty] private string? newNoteText;
	public bool IsEnabled => !string.IsNullOrEmpty(NewNoteTitle) && !string.IsNullOrEmpty(NewNoteText);

	public MainWindowViewModel()
	{
		Notes.Add(new NoteViewModel(new Note("Test note 1", "this is my text for test note 1")));
		Notes.Add(new NoteViewModel(new Note("Test note 2", "this is my text for test note 2")));
		Notes.Add(new NoteViewModel(new Note("Test note 3", "this is my text for test note 3")));
	}
	
	[RelayCommand]
	private void AddNewNote()
	{
		if (NewNoteTitle is not null && NewNoteText is not null)
		{
			var note = new NoteViewModel(new Note(NewNoteTitle, NewNoteText));
			Notes.Add(note);
			NewNoteTitle = null;
			NewNoteText = null;
		}
	}
	
	partial void OnNewNoteTitleChanged(string? oldValue, string? newValue)
		=> OnPropertyChanged(nameof(IsEnabled));

	partial void OnNewNoteTextChanged(string? oldValue, string? newValue)
		=> OnPropertyChanged(nameof(IsEnabled));
	
	

	
	
	
}