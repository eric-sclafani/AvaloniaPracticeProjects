using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services;

public static class ToDoListFileService
{
	private static readonly string _jsonFileName =
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"Avalonia.TodoApp", "MyToDoList.txt");

	public static async Task SaveToFileAsync(IEnumerable<TodoItem> itemsToSave)
	{
		// Ensure all directories exists
		Directory.CreateDirectory(Path.GetDirectoryName(_jsonFileName)!);

		// We use a FileStream to write all items to disc
		await using var fs = File.Create(_jsonFileName);
		await JsonSerializer.SerializeAsync(fs, itemsToSave);
	}

	// ...

	/// <summary>
	/// Loads the file from disc and returns the items stored inside
	/// </summary>
	/// <returns>An IEnumerable of items loaded or null in case the file was not found</returns>
	public static async Task<IEnumerable<TodoItem>?> LoadFromFileAsync()
	{
		try
		{
			// We try to read the saved file and return the ToDoItemsList if successful
			await using var fs = File.OpenRead(_jsonFileName);
			return await JsonSerializer.DeserializeAsync<IEnumerable<TodoItem>>(fs);
		}
		catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
		{
			return null;
		}
	}
}