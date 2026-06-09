using System.Text;
using System.Windows;
using System.Windows.Controls;
using ApiWorkbench.App.Configuration;
using ApiWorkbench.App.Services;
using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;

namespace ApiWorkbench.App;

public partial class MainWindow : Window
{
    private readonly ConnectionTestApiClient _apiClient;
    private bool _isLoadingProfile;

    public MainWindow()
    {
        InitializeComponent();

        var settings = AppSettingsLoader.Load();
        _apiClient = new ConnectionTestApiClient(settings.ApiBaseUrl);

        ConnectionTypeComboBox.SelectedIndex = 0;
    }

    private async void LoadProfilesButton_Click(object sender, RoutedEventArgs e)
    {
        await LoadProfilesAsync();
    }

    private async void SaveProfileButton_Click(object sender, RoutedEventArgs e)
    {
        SaveProfileButton.IsEnabled = false;
        StatusTextBlock.Text = "Saving profile...";

        try
        {
            var selectedProfile = SavedProfilesComboBox.SelectedItem as ConnectionProfile;
            var profile = BuildProfileFromForm(selectedProfile);

            var savedProfile = await _apiClient.SaveProfileAsync(profile);

            StatusTextBlock.Text = "Profile saved";
            ResultTextBox.Text = $"Saved profile: {savedProfile.Name}{Environment.NewLine}Id: {savedProfile.Id}";

            await LoadProfilesAsync(savedProfile.Id);
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = "Save error";
            ResultTextBox.Text = ex.Message;
        }
        finally
        {
            SaveProfileButton.IsEnabled = true;
        }
    }

    private async void DeleteProfileButton_Click(object sender, RoutedEventArgs e)
    {
        if (SavedProfilesComboBox.SelectedItem is not ConnectionProfile profile)
        {
            StatusTextBlock.Text = "No profile selected";
            ResultTextBox.Text = "Select a saved profile before deleting.";
            return;
        }

        var confirm = MessageBox.Show(
            $"Delete profile '{profile.Name}'?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes)
        {
            return;
        }

        DeleteProfileButton.IsEnabled = false;
        StatusTextBlock.Text = "Deleting profile...";

        try
        {
            await _apiClient.DeleteProfileAsync(profile.Id);

            SavedProfilesComboBox.SelectedItem = null;
            ProfileNameTextBox.Text = string.Empty;
            TargetTextBox.Text = string.Empty;
            ConnectionTypeComboBox.SelectedIndex = 0;

            ResultTextBox.Text = $"Deleted profile: {profile.Name}";
            StatusTextBlock.Text = "Profile deleted";

            await LoadProfilesAsync();
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = "Delete profile error";
            ResultTextBox.Text = ex.Message;
        }
        finally
        {
            DeleteProfileButton.IsEnabled = true;
        }
    }

    private async void RunTestButton_Click(object sender, RoutedEventArgs e)
    {
        RunTestButton.IsEnabled = false;
        StatusTextBlock.Text = "Running profile test...";
        ResultTextBox.Text = string.Empty;

        try
        {
            var selectedProfile = SavedProfilesComboBox.SelectedItem as ConnectionProfile;
            var profile = BuildProfileFromForm(selectedProfile);

            var result = await _apiClient.RunMockConnectionTestFromProfileAsync(profile);

            StatusTextBlock.Text = result.IsSuccess ? "Success" : "Failed";
            ResultTextBox.Text = FormatResult(result, "Profile-Based Mock Connection Test");

            await LoadHistoryAsync();
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = "Error";
            ResultTextBox.Text = ex.Message;
        }
        finally
        {
            RunTestButton.IsEnabled = true;
        }
    }

    private async void RunRestGetButton_Click(object sender, RoutedEventArgs e)
    {
        RunRestGetButton.IsEnabled = false;
        StatusTextBlock.Text = "Running REST GET test...";
        ResultTextBox.Text = string.Empty;

        try
        {
            var selectedProfile = SavedProfilesComboBox.SelectedItem as ConnectionProfile;
            var profile = BuildProfileFromForm(selectedProfile);

            var result = await _apiClient.RunRestApiGetTestFromProfileAsync(profile);

            StatusTextBlock.Text = result.IsSuccess ? "REST GET Success" : "REST GET Failed";
            ResultTextBox.Text = FormatResult(result, "Real REST GET Connection Test");

            await LoadHistoryAsync();
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = "REST GET error";
            ResultTextBox.Text = ex.Message;
        }
        finally
        {
            RunRestGetButton.IsEnabled = true;
        }
    }

    private async void LoadHistoryButton_Click(object sender, RoutedEventArgs e)
    {
        await LoadHistoryAsync();
    }

    private async void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
    {
        var confirm = MessageBox.Show(
            "Clear all connection test history?",
            "Confirm Clear History",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes)
        {
            return;
        }

        ClearHistoryButton.IsEnabled = false;
        StatusTextBlock.Text = "Clearing history...";

        try
        {
            await _apiClient.ClearHistoryAsync();

            HistoryDataGrid.ItemsSource = Array.Empty<ConnectionTestHistoryItem>();
            ResultTextBox.Text = "History cleared.";
            StatusTextBlock.Text = "History cleared";
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = "Clear history error";
            ResultTextBox.Text = ex.Message;
        }
        finally
        {
            ClearHistoryButton.IsEnabled = true;
        }
    }

    private void SavedProfilesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isLoadingProfile)
        {
            return;
        }

        if (SavedProfilesComboBox.SelectedItem is not ConnectionProfile profile)
        {
            return;
        }

        ProfileNameTextBox.Text = profile.Name;
        TargetTextBox.Text = profile.Target;
        SelectConnectionType(profile.ConnectionType);
        StatusTextBlock.Text = $"Loaded profile: {profile.Name}";
    }

    private void HistoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (HistoryDataGrid.SelectedItem is not ConnectionTestHistoryItem item)
        {
            return;
        }

        ResultTextBox.Text = FormatHistoryItem(item);
        StatusTextBlock.Text = $"Selected history item: {item.ProfileName}";
    }

    private async Task LoadProfilesAsync(Guid? selectProfileId = null)
    {
        LoadProfilesButton.IsEnabled = false;
        StatusTextBlock.Text = "Loading profiles...";

        try
        {
            var profiles = await _apiClient.GetProfilesAsync();

            _isLoadingProfile = true;
            SavedProfilesComboBox.ItemsSource = profiles;
            _isLoadingProfile = false;

            if (selectProfileId.HasValue)
            {
                SavedProfilesComboBox.SelectedItem = profiles.FirstOrDefault(profile => profile.Id == selectProfileId.Value);
            }

            StatusTextBlock.Text = $"Loaded {profiles.Count} profile(s)";
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = "Load error";
            ResultTextBox.Text = ex.Message;
        }
        finally
        {
            _isLoadingProfile = false;
            LoadProfilesButton.IsEnabled = true;
        }
    }

    private async Task LoadHistoryAsync()
    {
        LoadHistoryButton.IsEnabled = false;
        StatusTextBlock.Text = "Loading history...";

        try
        {
            var history = await _apiClient.GetHistoryAsync();

            HistoryDataGrid.ItemsSource = history;
            StatusTextBlock.Text = $"Loaded {history.Count} history item(s)";
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = "Load history error";
            ResultTextBox.Text = ex.Message;
        }
        finally
        {
            LoadHistoryButton.IsEnabled = true;
        }
    }

    private ConnectionProfile BuildProfileFromForm(ConnectionProfile? existingProfile)
    {
        var connectionType = GetSelectedConnectionType();

        return new ConnectionProfile
        {
            Id = existingProfile?.Id ?? Guid.NewGuid(),
            Name = ProfileNameTextBox.Text,
            ConnectionType = connectionType,
            Target = TargetTextBox.Text,
            Description = existingProfile?.Description ?? "Saved from WPF app",
            IsActive = true,
            CreatedAt = existingProfile?.CreatedAt ?? DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    private ConnectionType GetSelectedConnectionType()
    {
        var selectedItem = (ComboBoxItem)ConnectionTypeComboBox.SelectedItem;
        var connectionTypeText = selectedItem.Content?.ToString() ?? "Unknown";

        return Enum.TryParse<ConnectionType>(connectionTypeText, out var connectionType)
            ? connectionType
            : ConnectionType.Unknown;
    }

    private void SelectConnectionType(ConnectionType connectionType)
    {
        foreach (var item in ConnectionTypeComboBox.Items.OfType<ComboBoxItem>())
        {
            if (string.Equals(item.Content?.ToString(), connectionType.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ConnectionTypeComboBox.SelectedItem = item;
                return;
            }
        }

        ConnectionTypeComboBox.SelectedIndex = 0;
    }

    private static string FormatResult(ConnectionTestResult result, string title)
    {
        var errorText = string.IsNullOrWhiteSpace(result.ErrorMessage)
            ? "None"
            : result.ErrorMessage;

        var output = new StringBuilder();

        output.AppendLine(title);
        output.AppendLine(new string('-', title.Length));
        output.AppendLine($"Id: {result.Id}");
        output.AppendLine($"Profile Name: {result.ProfileName}");
        output.AppendLine($"Connection Type: {result.ConnectionType}");
        output.AppendLine($"Status: {result.Status}");
        output.AppendLine($"Message: {result.Message}");
        output.AppendLine($"Error: {errorText}");
        output.AppendLine($"Started At: {result.StartedAt}");
        output.AppendLine($"Completed At: {result.CompletedAt}");
        output.AppendLine($"Duration: {result.Duration}");
        output.AppendLine($"Is Success: {result.IsSuccess}");

        return output.ToString();
    }

    private static string FormatHistoryItem(ConnectionTestHistoryItem item)
    {
        var errorText = string.IsNullOrWhiteSpace(item.ErrorMessage)
            ? "None"
            : item.ErrorMessage;

        var output = new StringBuilder();

        output.AppendLine("Connection Test History Item");
        output.AppendLine("----------------------------");
        output.AppendLine($"Id: {item.Id}");
        output.AppendLine($"Profile Name: {item.ProfileName}");
        output.AppendLine($"Connection Type: {item.ConnectionType}");
        output.AppendLine($"Target: {item.Target}");
        output.AppendLine($"Status: {item.Status}");
        output.AppendLine($"Message: {item.Message}");
        output.AppendLine($"Error: {errorText}");
        output.AppendLine($"Started At: {item.StartedAt}");
        output.AppendLine($"Completed At: {item.CompletedAt}");
        output.AppendLine($"Duration: {item.Duration}");
        output.AppendLine($"Is Success: {item.IsSuccess}");

        return output.ToString();
    }
}
