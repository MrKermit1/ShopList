

using ShopList.Pages;

namespace ShopList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(AddNewProductPage), typeof(AddNewProductPage));
            Routing.RegisterRoute(nameof(AddNewShopPage), typeof(AddNewShopPage));
            Routing.RegisterRoute(nameof(AddNewCategoryPage), typeof(AddNewCategoryPage));
            Routing.RegisterRoute(nameof(CategoriesPage), typeof(CategoriesPage));
            Routing.RegisterRoute(nameof(ShopsPage), typeof(ShopsPage));
            Routing.RegisterRoute(nameof(ExportPage), typeof(ExportPage));
            Routing.RegisterRoute(nameof(RecipesPage), typeof(RecipesPage));
            Routing.RegisterRoute(nameof(AddNewRecipePage), typeof(AddNewRecipePage));

        }
    }
}
