namespace NotePad.Models;

public class Note
{
	public string Title { get; set; }
	public string Text { get; set; }

	public Note(string title, string text)
	{
		Title = title;
		Text = text;
	}
}