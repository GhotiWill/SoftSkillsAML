using Avalonia.Controls;
using Avalonia.Interactivity;
using SoftSkillsAML.Models;

namespace SoftSkillsAML;

public partial class AchievementAwardWindow : Window
{
    public AchievementAwardWindow(Achievement achievement)
    {
        InitializeComponent();
        DataContext = achievement;
    }

    private void CloseClick(object? sender, RoutedEventArgs e) => Close();
}
