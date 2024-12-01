namespace AoC.Services;

public class FileSystemCache
{
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    public async Task PutAsync(string fileName, string content)
    {
        // If the lock is already taken, return immediately
        if (_writeLock.CurrentCount == 0)
            return;

        await _writeLock.WaitAsync();

        var directory = fileName[..^fileName.LastIndexOf('/')];
        if (Directory.Exists(directory) is false)
            Directory.CreateDirectory(directory);

        await File.WriteAllTextAsync(fileName, content);
        _writeLock.Release();
    }

    public async Task<string?> LookupAsync(string fileName)
        => File.Exists(fileName) 
        ? await File.ReadAllTextAsync(fileName) 
        : null;
}
