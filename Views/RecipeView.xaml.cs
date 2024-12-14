namespace ShopList.Views;
using ShopList.Models;
using ShopList.Files;
using ShopList.Pages;
public partial class RecipeView : ContentView
{
    public FileManager FileManager { get; set; } = new();
    public static readonly BindableProperty RecipeModelProperty =
    BindableProperty.Create(
        nameof(RecipeModel),
        typeof(RecipeModel),
        typeof(RecipeView),
        default(RecipeModel),
        BindingMode.TwoWay,
        propertyChanged: OnRecipeModelChanged
    );

    public RecipeModel RecipeModel
    {
        get => (RecipeModel)GetValue(RecipeModelProperty);
        set
        {
            SetValue(RecipeModelProperty, value);
            BindingContext = value;
        }
    }

    public void FillProducts()
    {
        RecipeModel.FillProducts();
    }

    private static void OnRecipeModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RecipeView recipeView)
        {
            recipeView.BindingContext = newValue;
        }
    }

    public RecipeView()
	{
		InitializeComponent();
        RecipeModel = new();
        BindingContext = this;
        FillProducts();
	}

    public RecipeView(RecipeModel model)
    {
        InitializeComponent();
        BindingContext = this;
        RecipeModel = model;
        FillProducts();
    }

    private async void Refresh()
    {
        var navigationStack = Navigation.NavigationStack;
        var currentRoute = Shell.Current?.CurrentState?.Location?.ToString();
        //nowy main
        if (currentRoute == "//MainPage/RecipesPage")
        {
            if (navigationStack.Count > 0)
            {
                var oldMainPage = navigationStack.LastOrDefault(p => p is RecipesPage);
                //usun starego maina
                if (oldMainPage != null)
                {
                    Navigation.RemovePage(oldMainPage);
                }
            }

            var main = new RecipesPage();
            await Navigation.PushAsync(main);
            await Shell.Current.GoToAsync("RecipesPage");
        }
    }

    private void RecipeClicked(object sender, EventArgs e)
    {
        foreach (var product in RecipeModel.Products)
        {
            FileManager.AddProduct(product);
        }

        Shell.Current.GoToAsync("//MainPage");
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        FileManager.DeleteRecipeByName(RecipeModel.Name);
        Refresh();
    }
}