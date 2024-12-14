using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopList.Files;
using System.Xml.Serialization;

namespace ShopList.Models
{
    public class RecipeModel
    {
        public ObservableCollection<ProductModel> Products { get; set; } = new();

        [XmlAttribute("Name")]
        public string Name { get; set; } = String.Empty;
        public void FillProducts()
        {
            FileManager fileManager = new();

            foreach (var product in fileManager.LoadProductsFromRecipe(Name))
            {
                Products.Add(product);
            }
        }
    }
}
