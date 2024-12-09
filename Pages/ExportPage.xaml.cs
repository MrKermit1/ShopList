namespace ShopList.Pages;
using ShopList.Files;
using ShopList.Models;
using CommunityToolkit.Maui.Storage;
public partial class ExportPage : ContentPage
{
	private FileManager FileManager { get; set; } = new();
	public string Name {  get; set; } = String.Empty;
    public string Path { get; set; } = String.Empty;

    public ExportPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

    private void OnExport(object sender, EventArgs e)
    {
        if (Name.Trim() != "")
        {
            FileManager.ExportProductsToXml(Name, FileManager.LoadProducts());
            Shell.Current.GoToAsync("//MainPage");
        }
    }
}