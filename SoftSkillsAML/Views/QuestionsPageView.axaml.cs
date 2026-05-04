using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class QuestionsPageView : UserControl
{
    public QuestionsPageView(int departmentId)
    {
        InitializeComponent();
        DataContext = new QuestionsPageViewModel(departmentId);
    }

    private void QuestionDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is QuestionsPageViewModel vm) vm.OpenQuestion();
    }
}
