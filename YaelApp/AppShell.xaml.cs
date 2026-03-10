namespace YaelApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        var gifPageType = Type.GetType("YaelApp.Pages.GifDuJourPage, YaelApp");
        if (gifPageType is not null)
        {
            Routing.RegisterRoute("GifDuJourPage", gifPageType);
        }
    }
}