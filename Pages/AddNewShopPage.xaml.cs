namespace ShopList.Pages;
using ShopList.Models;
using ShopList.Files;
using System.Collections.ObjectModel;
public partial class AddNewShopPage : ContentPage
{
	FileManager FileManager = new();
	ShopModel ShopModel {get;set;}

	
	public AddNewShopPage()
	{
		InitializeComponent();
		ShopModel = new ShopModel
		{ 
			Name = ""
		};

		BindingContext = ShopModel;

	}

    private async void OnAddShopClicked(object sender, EventArgs e)
    {
		if (ShopModel.Name.Trim() != "")
		{
            if (FileManager.CheckIfShopExist(ShopModel.Name))
            {
                await DisplayAlert("Shop hasn't been added", $"Shop already exist", "OK");
				return;
            }

            FileManager.AddShop(ShopModel);
			await DisplayAlert("Shop has been added", $"Name:{ShopModel.Name}", "OK");
			await Shell.Current.GoToAsync("//MainPage");
		}
		else
		{
            await DisplayAlert("Shop hasn't been added", $"Name shouldn't be empty{ShopModel.Name}", "OK");
        }
    }
}