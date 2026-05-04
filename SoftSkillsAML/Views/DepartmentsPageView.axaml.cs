using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class DepartmentsPageView : UserControl
{
    public DepartmentsPageView()
    {
        InitializeComponent();
        DataContext = new DepartmentsPageViewModel();
    }

    private void DepartmentDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is DepartmentsPageViewModel vm) vm.OpenDepartment();
    }
}