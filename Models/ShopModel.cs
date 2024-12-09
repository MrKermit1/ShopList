using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CommunityToolkit.Maui.Core.Extensions;
using ShopList.Files;
namespace ShopList.Models
{
    public class ShopModel
    {
        [XmlAttribute("Name")]
        public string Name { get; set; } = String.Empty;
        public ObservableCollection<ProductModel> Products { get; set; } = new();
        public void FillProducts()
        {
            Products.Clear();
            FileManager fileManager = new();
            foreach (var product in fileManager.LoadProducts().Where(p => p.Check == false && p.Shop == Name))
            {
                Products.Add(product);
            }

            Products = Products.OrderBy(p => p.Category).ToObservableCollection<ProductModel>();
        }
    }
}
