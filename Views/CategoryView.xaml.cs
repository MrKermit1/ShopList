namespace ShopList.Views;
using ShopList.Files;
using ShopList.Models;
using System.Collections.ObjectModel;

public partial class CategoryView : ContentView
{
    public static readonly BindableProperty CategoryModelProperty =
    BindableProperty.Create(
        nameof(CategoryModel),
        typeof(CategoryModel),
        typeof(CategoryView),
        default(CategoryModel),
        BindingMode.TwoWay,
        propertyChanged: OnCategoryModelChanged
    );

    public CategoryModel CategoryModel
    {
        get => (CategoryModel)GetValue(CategoryModelProperty);
        set
        {
            SetValue(CategoryModelProperty, value);
            BindingContext = value;
        }
    }

    private static void OnCategoryModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CategoryView categoryView)
        {
            categoryView.BindingContext = newValue;
        }
    }

    public void FillProducts()
    {
        CategoryModel.FillProducts();
    }

    public CategoryView()
	{
		InitializeComponent();
        CategoryModel = new();
        sortPicker.SelectedIndex = 0;
        BindingContext = this;
        FillProducts();
	}

    public CategoryView(CategoryModel model)
    {
        InitializeComponent();
        BindingContext = this;
        sortPicker.SelectedIndex = 0;
        CategoryModel = model;
        FillProducts();
    }

    private void SortMethodChange(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {

            IEnumerable<ProductModel> sortedProducts = CategoryModel.Products;

            switch (selectedIndex)
            {
                case 0:
                    FillProducts();
                    break;
                case 1:
                    sortedProducts = CategoryModel.Products.OrderBy(p => p.Name);
                    break;
                case 2:
                    sortedProducts = CategoryModel.Products.OrderByDescending(p => p.Quanity);
                    break;
                case 3:
                    sortedProducts = CategoryModel.Products.OrderBy(p => p.Quanity);
                    break;
                default:
                    break;
            }

            var sortedList = sortedProducts.ToList();
            CategoryModel.Products.Clear();
            foreach (var product in sortedList)
            {
                CategoryModel.Products.Add(product);
            }
        }
    }
}
