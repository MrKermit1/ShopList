namespace ShopList.Views;
using ShopList.Models;
using ShopList.Files;
using ShopList.Pages;
public partial class ProductView : ContentView
{
    public static readonly BindableProperty ProductModelProperty =
        BindableProperty.Create(
            nameof(ProductModel),
            typeof(ProductModel),
            typeof(ProductView),
            default(ProductModel),
            BindingMode.TwoWay,
            propertyChanged: OnProductModelChanged
        );

    public FileManager FileManager = new();

    public ProductModel ProductModel
    {
        get => (ProductModel)GetValue(ProductModelProperty);
        set => SetValue(ProductModelProperty, value);
    }
    
    public ProductView()
    {
        InitializeComponent();
    }

    private async void Refresh()
    {
        var navigationStack = Navigation.NavigationStack;
        var currentRoute = Shell.Current?.CurrentState?.Location?.ToString();

        if (currentRoute ==  "//MainPage")
        {
            if (navigationStack.Count > 0)
            {
                var oldMainPage = navigationStack.LastOrDefault(p => p is MainPage);

                if (oldMainPage != null)
                {
                    Navigation.RemovePage(oldMainPage);
                }
            }

            var main = new MainPage();
            await Navigation.PushAsync(main);
            await Shell.Current.GoToAsync("//MainPage");
        }

        if (currentRoute == "//MainPage/CategoriesPage")
        {
            if (navigationStack.Count > 0)
            {
                var oldMainPage = navigationStack.LastOrDefault(p => p is CategoriesPage);

                if (oldMainPage != null)
                {
                    Navigation.RemovePage(oldMainPage);
                }
            }

            var main = new CategoriesPage();
            await Navigation.PushAsync(main);
            await Shell.Current.GoToAsync("CategoriesPage");
        }

        if (currentRoute == "//MainPage/ShopsPage")
        {

            if (navigationStack.Count > 0)
            {
                var oldMainPage = navigationStack.LastOrDefault(p => p is ShopsPage);

                if (oldMainPage != null)
                {
                    Navigation.RemovePage(oldMainPage);
                }
            }

            var main = new ShopsPage();
            await Navigation.PushAsync(main);
            await Shell.Current.GoToAsync("ShopsPage");
        }

    }

    public ProductView(ProductModel	model)
	{
		InitializeComponent();
		ProductModel = model;
		BindingContext = ProductModel;
	}

	private static void OnProductModelChanged(BindableObject bindable, object oldValue, object newValue)
	{
        if (bindable is ProductView productView)
        {
			productView.BindingContext = newValue;
        }
    }

    private void OnIncrementClicked(object sender, EventArgs e)
    {
        ProductModel.Increment();
        FileManager.UpdateProduct(ProductModel);
    }

    private void OnDecrementClicked(object sender, EventArgs e)
    {
        if (ProductModel.Quanity > 0)
        {
            ProductModel.Decrement();
            FileManager.UpdateProduct(ProductModel);
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        FileManager.DeleteProductByName(ProductModel.Name);
        Refresh();
    }

    private void OnDoneClicked(object sender, EventArgs e)
    {
        if (!ProductModel.Check)
        {
            ProductModel.BgColor = "DarkGrey";
            ProductModel.Check = true;
            //bo nowe zawsze idzie na dó³
            FileManager.DeleteProductByName(ProductModel.Name);
            FileManager.AddProduct(ProductModel);
            Refresh();
        }
    }
}