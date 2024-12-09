namespace ShopList.Pages;
using ShopList.Files;
using ShopList.Models;
public partial class AddNewCategoryPage : ContentPage
{
	public FileManager FileManager = new();
	public CategoryModel CategoryModel {  get; set; }
	public AddNewCategoryPage()
	{
		InitializeComponent();

        CategoryModel = new CategoryModel
        {
            Name = "",
        };

		BindingContext = CategoryModel;
	}

    private bool isCategoryExist()
    {
        foreach(var category in FileManager.LoadCategories())
        {
            if (category.Name == CategoryModel.Name)
            {
                return true;
            }
        }

        return false;
    }

    private async void OnAddCategoryClicked(object sender, EventArgs e)
    {
        if (!isCategoryExist()) 
        {
            if (CategoryModel.Name.Trim() != "")
            {
                FileManager.AddCategory(CategoryModel);
                await DisplayAlert("Category has been added",
                        $"Name: {CategoryModel.Name}\n", "OK");

                BindingContext = this;
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await DisplayAlert("Category hasn't been added", "Name shouldn't be empty", "OK");
            }
        }
        else
        {
            await DisplayAlert("Category hasn't been added", "Category already exist", "OK");
        }
    }
}