using System.IO;
using System.Text.Json;

namespace ApiWorkbench.App.Configuration;

public static class AppSettingsLoader
{
    public static AppSettings Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        if (!File.Exists(path))
        {
            return new AppSettings();
        }

        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<AppSettings>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new AppSettings();
    }
}
