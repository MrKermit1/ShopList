using ShopList.Files;
using ShopList.Models;
using System.Collections.ObjectModel;

namespace ShopList.Pages;

public partial class AddNewRecipePage : ContentPage
{
	public FileManager FileManager = new();
	public ObservableCollection<ProductModel> Products { get; set; } = new();
	public ObservableCollection<ProductModel> ProductsToRecipe { get; set; } = new();

	public string FileName { get; set; }
	public string Desc { get; set; }

	public AddNewRecipePage()
	{
		InitializeComponent();
		BindingContext = this;

		FillProducts();
	}

	private void FillProducts()
	{
        Products.Clear();
        foreach (var product in FileManager.LoadProducts())
		{
			Products.Add(product);
		}
	}

    private void OnProductCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if (sender is CheckBox checkBox && checkBox.BindingContext is ProductModel product)
		{
			var productName = product.Name;
			ProductsToRecipe.Add(product);
        }
    }

    private void AddRecipeClicked(object sender, EventArgs e)
    {

        if (FileManager.CheckIfRecipeExist(FileName))
        {
			DisplayAlert("Recipe hasn't been added", "Recipe already exist", "OK");
			return;
        }

        if (Products.Count == 0 || ProductsToRecipe.Count == 0)
		{
            DisplayAlert("Recipe hasn't been added", "There is no products to add", "OK");
            return;
		}

		FileManager.SaveRecipe(FileName, ProductsToRecipe);
		FileManager.SaveRecipeDesc(FileName, Desc);

		string msg = string.Empty;
		foreach (var product in ProductsToRecipe)
		{
			msg += "\n" + product.Name + "\n";
		}

		DisplayAlert("Added Recipe", "Products from Recipe:" + msg, "OK");

		Shell.Current.GoToAsync("//MainPage");
    }

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
		Desc = ((Editor)sender).Text;
    }
}