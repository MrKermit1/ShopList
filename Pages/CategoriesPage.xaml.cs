namespace ShopList.Pages;
using ShopList.Files;
using ShopList.Models;
using System.Collections.ObjectModel;

public partial class CategoriesPage : ContentPage
{
	public FileManager FileManager { get; set; } = new();
    public ObservableCollection<CategoryModel> Categories { get; set; } = new();
    public CategoriesPage()
	{
		InitializeComponent();
        BindingContext = this;
        FillCategories();
    }

    private void FillCategories()
    {
        Categories.Clear();
        foreach (var category in FileManager.LoadCategories())
        {
            category.FillProducts();
            if (category.Products.Count > 0)
            {
                Categories.Add(category);
            }
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        FillCategories();
    }
}