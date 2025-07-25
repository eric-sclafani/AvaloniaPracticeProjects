using System.Text.Json;
using iTunesSearch.Library;

namespace MusicStore.Models;

public class Album
{
	private static readonly iTunesSearchManager s_SearchManager = new();
	
	public string Artist { get; set; }
	public string Title { get; set; }
	public string CoverUrl { get; set; }
	
	private static readonly HttpClient s_httpClient = new();
	private string  CachePath => $"./Cache/{SanitizeFileName(Artist)} - {SanitizeFileName(Title)}";
	
	public Album(string artist, string title, string coverUrl)
	{
		Artist = artist;
		Title = title;
		CoverUrl = coverUrl;
	}

	// sanitizes input to replace characters that cannot be used in the file name with _
	private static string SanitizeFileName(string input)
	{
		return Path.GetInvalidFileNameChars().Aggregate(input, (current, c) => current.Replace(c, '_'));
	}

	public async Task<Stream> LoadCoverBitmapAync()
	{
		if (File.Exists(CachePath + ".bmp"))
		{
			return File.OpenRead(CachePath + ".bmp");
		}
		
		var data = await s_httpClient.GetByteArrayAsync(CoverUrl);
		return new MemoryStream(data);
	}

	public static async Task<IEnumerable<Album>> SearchAsync(string? searchTerm)
	{
		if (string.IsNullOrEmpty(searchTerm))
		{
			return [];
		}
		
		var query = await s_SearchManager.GetAlbumsAsync(searchTerm).ConfigureAwait(false);

		return query.Albums.Select(x =>
			new Album(x.ArtistName, x.CollectionName, x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
	}

	public async Task SaveAsync()
	{
		if (!Directory.Exists("./Cache"))
		{
			Directory.CreateDirectory("./Cache");
		}

		await using var fs = File.OpenWrite(CachePath);
		await SaveToStreamAsync(this, fs);
	}
	
	public Stream SaveCoverBitmapStream()
	{
		return File.OpenWrite(CachePath + ".bmp");
	}

	private static async Task SaveToStreamAsync(Album data, Stream stream)
	{
		await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
	}
	
	public static async Task<Album> LoadFromStream(Stream stream)
	{
		return (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;
	}

	public static async Task<IEnumerable<Album>> LoadCachedAsync()
	{
		if (!Directory.Exists("./Cache"))
		{
			Directory.CreateDirectory("./Cache");
		}

		var results = new List<Album>();

		foreach (var file in Directory.EnumerateFiles("./Cache"))
		{
			if (!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;

			await using var fs = File.OpenRead(file);
			results.Add(await LoadFromStream(fs).ConfigureAwait(false));
		}

		return results;
	}
}