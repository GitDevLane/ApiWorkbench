using System.Text;
using System.Windows;
using System.Windows.Controls;
using ApiWorkbench.App.Services;
using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;

namespace ApiWorkbench.App;

public partial class MainWindow : Window
{
    private readonly ConnectionTestApiClient _apiClient = new("http://localhost:5075");

    public MainWindow()
    {
        InitializeComponent();
        ConnectionTypeComboBox.SelectedIndex = 0;
    }

    private async void RunTestButton_Click(object sender, RoutedEventArgs e)
    {
        RunTestButton.IsEnabled = false;
        StatusTextBlock.Text = "Running profile test...";
        ResultTextBox.Text = string.Empty;

        try
        {
            var selectedItem = (ComboBoxItem)ConnectionTypeComboBox.SelectedItem;
            var connectionTypeText = selectedItem.Content?.ToString() ?? "Unknown";

            if (!Enum.TryParse<ConnectionType>(connectionTypeText, out var connectionType))
            {
                connectionType = ConnectionType.Unknown;
            }

            var profile = new ConnectionProfile
            {
                Name = ProfileNameTextBox.Text,
                ConnectionType = connectionType,
                Target = TargetTextBox.Text,
                Description = "Temporary WPF profile test",
                IsActive = true
            };

            var result = await _apiClient.RunMockConnectionTestFromProfileAsync(profile);

            StatusTextBlock.Text = result.IsSuccess ? "Success" : "Failed";

            var output = new StringBuilder();
            output.AppendLine("Profile-Based Mock Connection Test");
            output.AppendLine("----------------------------------");
            output.AppendLine($"Id: {result.Id}");
            output.AppendLine($"Profile Name: {result.ProfileName}");
            output.AppendLine($"Connection Type: {result.ConnectionType}");
            output.AppendLine($"Status: {result.Status}");
            output.AppendLine($"Message: {result.Message}");
            output.AppendLine($"Error: {result.ErrorMessage}");
            output.AppendLine($"Started At: {result.StartedAt}");
            output.AppendLine($"Completed At: {result.CompletedAt}");
            output.AppendLine($"Duration: {result.Duration}");
            output.AppendLine($"Is Success: {result.IsSuccess}");

            ResultTextBox.Text = output.ToString();
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
}
