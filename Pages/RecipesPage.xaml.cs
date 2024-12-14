using System.Collections.ObjectModel;
using ShopList.Files;
using ShopList.Models;
namespace ShopList.Pages;

public partial class RecipesPage : ContentPage
{
    public FileManager FileManager { get; set; } = new();
    public ObservableCollection<RecipeModel> Recipes { get; set; } = new();
    public static RecipesPage Instance { get; set; }

    public RecipesPage()
	{
		InitializeComponent();
        Instance = this;
        BindingContext = this;
        FillRecipes();
    }

    public void FillRecipes()
    {
        foreach (var name in FileManager.LoadRecipesNames())
        {
            RecipeModel model = new RecipeModel
            {
                Name = name
            };
            model.FillProducts();
            Recipes.Add(model);
        }
    }

}