using System.Collections.ObjectModel;
using ShopList.Models;
using ShopList.Files;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityToolkit.Maui.Storage;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
namespace ShopList.Pages
{
    public partial class MainPage : ContentPage
    {
        public  ObservableCollection<ProductModel> Products { get; set; } = new();
        public  FileManager FileManager { get; set; } = new();
        public static MainPage Instance { get; set; }
        public MainPage()
        {
            InitializeComponent();
            Instance = this;
            BindingContext = this;
            sortPicker.SelectedIndex = 0;
            FillProducts();
        }

        public void FillProducts()
        {
            Products.Clear();
            foreach (var product in FileManager.LoadProducts().Where(p => p.Check == false)) 
            { 
                Products.Add(product);
            }

            foreach (var product in FileManager.LoadProducts().Where(p => p.Check == true))
            {
                Products.Add(product);
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            FillProducts();
        }


        async Task SaveFile(CancellationToken cancellationToken)
        {
            var serializer = new XmlSerializer(typeof(List<ProductModel>));
            using var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.Default, leaveOpen: true))
            {
                serializer.Serialize(writer, new List<ProductModel>(FileManager.LoadProducts()));
            }

            var fileSaverResult = await FileSaver.Default.SaveAsync("list.xml", stream, cancellationToken);
            if (fileSaverResult.IsSuccessful)
            {
                await Shell.Current.DisplayAlert("Export info", "The file was saved successfully", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Export info", $"The file was not saved successfully with error: {fileSaverResult.Exception.Message}", "OK");
            }
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
            if (Products.Count() != 0)
            {
                await SaveFile(CancellationToken.None);
            }
            else
            {
                await Shell.Current.DisplayAlert("Export info", "There is no products to export", "OK");
            }
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
                        FillProducts();
                        break;
                    case 1:
                        sortedProducts = Products.OrderBy(p => p.Name);
                        break;
                    case 2:
                        sortedProducts = Products.OrderBy(p => p.Category);
                        break;
                    case 3:
                        sortedProducts = Products.OrderByDescending(p => p.Quanity);
                        break;
                    case 4:
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
