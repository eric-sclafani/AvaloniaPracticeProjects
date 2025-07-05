using CommunityToolkit.Mvvm.ComponentModel;
using NotePad.Models;

namespace NotePad.ViewModels;

public partial class NoteViewModel : ViewModelBase
{
	private readonly Note _note;

	public NoteViewModel(Note note)
	{
		_note = note;
	}

	public string Title => _note.Title;
	public string Text => _note.Text;
	
	
	

}