using YaelApp.ViewModels;
namespace YaelApp.Pages;
public partial class ScoreDetailPage : ContentPage
{
    public ScoreDetailPage(ScoreDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
