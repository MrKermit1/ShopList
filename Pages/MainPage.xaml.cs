using System.Collections.ObjectModel;
using ShopList.Models;
using ShopList.Files;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityToolkit.Maui.Storage;
using System.Linq;
namespace ShopList.Pages
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<ProductModel> Products { get; set; } = new();
        public FileManager FileManager { get; set; } = new();
        public string SortBy {  get; set; } = String.Empty;
        public MainPage()
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            FillProducts();
        }

        private async void OnAddNewProduct(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AddNewProductPage");
        }

        private async void OnAddNewShop(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AddNewShopPage");
        }

        private async void OnAddNewCategory(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AddNewCategoryPage");
        }

        private async void OnCategories(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("CategoriesPage");
        }

        private async void OnListToShop(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ShopsPage");
        }

        private async void OnExport(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ExportPage");
        }
        private async void OnRecipes(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("RecipesPage");
        }
        private async void OnAddNewRecipe(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AddNewRecipePage");
        }
        private async void OnImport(object sender, EventArgs e)
        {
            var result = await FilePicker.Default.PickAsync(default);
            if (result !=null && result.FileName.EndsWith("xml", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var product in FileManager.ImportProducts(result.FullPath))
                {
                    Products.Add(product);
                }
                await DisplayAlert("Import info", "Import done", "OK");
            }
            else 
            {
                await DisplayAlert("Import info", "Import error ", "OK");
            }
        }

        private void SortMethodChange(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {

                IEnumerable<ProductModel> sortedProducts = Products;

                switch (selectedIndex)
                {
                    case 0:
                        sortedProducts = Products.OrderBy(p => p.Name);
                        break;
                    case 1:
                        sortedProducts = Products.OrderBy(p => p.Category);
                        break;
                    case 2:
                        sortedProducts = Products.OrderByDescending(p => p.Quanity);
                        break;
                    case 3:
                        sortedProducts = Products.OrderBy(p => p.Quanity);
                        break;
                    default:
                        break;
                }

                var sortedList = sortedProducts.ToList();
                Products.Clear();
                foreach (var product in sortedList)
                {
                    Products.Add(product);
                }

            }
        }
    }

}
