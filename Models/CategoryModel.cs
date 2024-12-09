using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ShopList.Files;
namespace ShopList.Models
{
    public class CategoryModel
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }  = String.Empty;

        public ObservableCollection<ProductModel> Products { get; set; } = new();

        public void FillProducts()
        {
            Products.Clear();
            FileManager fileManager = new FileManager();
            foreach (var product in fileManager.LoadProducts().Where(p => p.Category == Name))
            {
                Products.Add(product);
            }
        }
    }
}
