using YaelApp.ViewModels;

namespace YaelApp.Pages;

public partial class VolleyScorePage : ContentPage
{
    public VolleyScorePage(VolleyScoreViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
