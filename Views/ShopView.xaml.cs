namespace ShopList.Views;
using ShopList.Files;
using ShopList.Models;
using System.Diagnostics;

public partial class ShopView : ContentView
{
    public static readonly BindableProperty ShopModelProperty =
    BindableProperty.Create(
        nameof(ShopModel),
        typeof(ShopModel),
        typeof(ShopView),
        default(ShopModel),
        BindingMode.TwoWay,
        propertyChanged: OnShopModelChanged
    );

    public ShopModel ShopModel
    {
        get => (ShopModel)GetValue(ShopModelProperty);
        set
        {
            SetValue(ShopModelProperty, value);
            BindingContext = value;
        }
    }

    private static void OnShopModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ShopView shopView)
        {
            shopView.BindingContext = newValue;
        }
    }

    public void FillProducts()
    {
        ShopModel.FillProducts();
    }

    public ShopView()
    {
        InitializeComponent();
        ShopModel = new();
        BindingContext = this;
        FillProducts();
    }

    public ShopView(ShopModel model)
    {
        InitializeComponent();
        BindingContext = this;
        ShopModel = model;
        FillProducts();
    }

    private void SortMethodChange(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {

            IEnumerable<ProductModel> sortedProducts = ShopModel.Products;

            switch (selectedIndex)
            {
                case 0:
                    sortedProducts = ShopModel.Products.OrderBy(p => p.Name);
                    break;
                case 1:
                    sortedProducts = ShopModel.Products.OrderBy(p => p.Category);
                    break;
                case 2:
                    sortedProducts = ShopModel.Products.OrderByDescending(p => p.Quanity);
                    break;
                case 3:
                    sortedProducts = ShopModel.Products.OrderBy(p => p.Quanity);
                    break;
                default:
                    break;
            }

            var sortedList = sortedProducts.ToList();
            ShopModel.Products.Clear();
            foreach (var product in sortedList)
            {
                ShopModel.Products.Add(product);
            }

        }
    }
}