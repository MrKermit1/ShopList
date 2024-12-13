using ShopList.Models;
using ShopList.Files;
using System.Collections.ObjectModel;
namespace ShopList.Pages;

public partial class AddNewProductPage : ContentPage
{

	public ProductModel ProductModel { get; set; }
	public FileManager FileManager { get; set; } = new();

    public ObservableCollection<string> CategoryStrings { get; set; } = new();
    public ObservableCollection<string> ShopStrings { get; set; } = new();
    public void FillCategories()
    {
        CategoryStrings.Clear();
        foreach (var category in FileManager.LoadCategories())
        {
            CategoryStrings.Add(category.Name);
        }
    }

    public void FillShops()
    {
        ShopStrings.Clear();
        foreach (var shop in FileManager.LoadShops())
        {
            ShopStrings.Add(shop.Name);
        }
    }

	public AddNewProductPage()
	{
		InitializeComponent();

        ProductModel = new ProductModel
        {
            Name = "",
            Shop = "",
            Unit = "",
            Quanity = 0,
            Check = false,
            BgColor = "DarkSeaGreen",
            Category = ""
        };

        FillCategories();
        FillShops();

        BindingContext = this;

    }

    private bool LookForEmptyFields()
    {
        return
            String.IsNullOrEmpty(ProductModel.Name) ||
            String.IsNullOrEmpty(ProductModel.Unit) ||
            String.IsNullOrEmpty(ProductModel.Shop) ||
            String.IsNullOrEmpty(ProductModel.Category);
    }

    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        if (!LookForEmptyFields())
        {
            if (FileManager.CheckIfProductExist(ProductModel.Name))
            {
                await DisplayAlert("Product hasn't been added", "This product already has been added", "OK");
                return;
            }

            if (ProductModel.Optional)
            {
                ProductModel.BgColor = "Brown";
            }

            FileManager.AddProduct(ProductModel);
            await DisplayAlert("Product has been added",
                    $"Name: {ProductModel.Name}\n" +
                    $"Shop: {ProductModel.Shop}\n" +
                    $"Unit: {ProductModel.Unit}\n" +
                    $"Category: {ProductModel.Category}\n" +
                    $"Quanity: {ProductModel.Quanity.ToString()}", "OK");

            BindingContext = this;
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await DisplayAlert("Product hasn't been added", "Form hasn't been filled properly", "OK");
        }
    }

    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        ProductModel.Optional = e.Value;
    }

}