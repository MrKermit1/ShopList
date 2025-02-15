namespace ShopList.Pages;
using ShopList.Files;
using ShopList.Models;
using System.Collections.ObjectModel;
public partial class ShopsPage : ContentPage
{
    public FileManager FileManager { get; set; } = new();
    public ObservableCollection<ShopModel> Shops { get; set; } = new();

    public static ShopsPage Instance { get; set; }
    public void FillShops()
    {
        Shops.Clear();
        foreach (var shop in FileManager.LoadShops())
        {
            shop.FillProducts();
            if (shop.Products.Count > 0)
            {
                Shops.Add(shop);
            }
        }
    }

    public ShopsPage()
	{
		InitializeComponent();
        Instance = this;
        BindingContext = this;
        FillShops();
	}
}