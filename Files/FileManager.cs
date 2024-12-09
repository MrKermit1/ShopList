using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ShopList.Models;

namespace ShopList.Files
{
    public class FileManager
    {

        private static readonly string productsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.xml");
        private static readonly string shopsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shops.xml");
        private static readonly string categoriesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "categories.xml");
        private static readonly string exportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "exports");
        private static readonly string recipesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recipes");

        public FileManager(){}

        private void CreateXml(string path, string root)
        {
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                writer.WriteStartElement(root, "");

                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        //SAVE

        public void SaveProducts(ObservableCollection<ProductModel> products)
        {
            if (!File.Exists(productsPath))
            {
                CreateXml(productsPath, "products");
            }

            XmlSerializer serializer = new(typeof(List<ProductModel>), new XmlRootAttribute("products"));

            using (StreamWriter writer = new(productsPath))
            {
                serializer.Serialize(writer, new List<ProductModel>(products));
            }
        }

        public void SaveShops(ObservableCollection<ShopModel> shops)
        {
            if (!File.Exists(shopsPath))
            {
                CreateXml(productsPath, "shops");
            }

            XmlSerializer serializer = new(typeof(List<ShopModel>), new XmlRootAttribute("shops"));

            using (StreamWriter writer = new(shopsPath))
            {
                serializer.Serialize(writer, new List<ShopModel>(shops));
            }
        }

        public void SaveCategories(ObservableCollection<CategoryModel> categories)
        {
            if (!File.Exists(shopsPath))
            {
                CreateXml(productsPath, "shops");
            }

            XmlSerializer serializer = new(typeof(List<CategoryModel>), new XmlRootAttribute("categories"));

            using (StreamWriter writer = new(categoriesPath))
            {
                serializer.Serialize(writer, new List<CategoryModel>(categories));
            }
        }

        public void SaveRecipe(string fileName, ObservableCollection<ProductModel> products)
        {
            var path = Path.Combine(recipesPath, fileName + ".xml");
            CreateXml(path, "products");

            XmlSerializer serializer = new(typeof(List<ProductModel>), new XmlRootAttribute("products"));

            using (StreamWriter writer = new(path))
            {
                serializer.Serialize(writer, new List<ProductModel>(products));
            }
        }

        public void ExportProductsToXml(string fileName, ObservableCollection<ProductModel> products)
        {
            var path = Path.Combine(exportPath, fileName + ".xml");

            if (!File.Exists(path))
            {
                CreateXml(path, "products");

                XmlSerializer serializer = new(typeof(List<ProductModel>), new XmlRootAttribute("products"));

                using (StreamWriter writer = new(path))
                {
                    serializer.Serialize(writer, new List<ProductModel>(products));
                }
            }
        }

        //LOAD

        public ObservableCollection<ProductModel> LoadProducts()
        {
            if (!File.Exists(productsPath))
            {
                return new ObservableCollection<ProductModel>();
            }

            XmlSerializer serializer = new(typeof(List<ProductModel>), new XmlRootAttribute("products"));

            using (StreamReader reader = new(productsPath))
            {
                List<ProductModel> products = (List<ProductModel>)serializer.Deserialize(reader);
                CheckCategories(products);
                CheckShops(products);
                return new ObservableCollection<ProductModel>(products);
            }
        }

        public ObservableCollection<ShopModel> LoadShops()
        {
            if (!File.Exists(shopsPath))
            {
                return new ObservableCollection<ShopModel>();
            }

            XmlSerializer serializer = new(typeof(List<ShopModel>), new XmlRootAttribute("shops"));

            using (StreamReader reader = new(shopsPath))
            {
                List<ShopModel> shops = (List<ShopModel>)serializer.Deserialize(reader);
                return new ObservableCollection<ShopModel>(shops);
            }
        }

        public ObservableCollection<CategoryModel> LoadCategories()
        {
            if (!File.Exists(categoriesPath))
            {
                return new ObservableCollection<CategoryModel>();
            }

            XmlSerializer serializer = new(typeof(List<CategoryModel>), new XmlRootAttribute("categories"));

            using (StreamReader reader = new(categoriesPath))
            {
                List<CategoryModel> categories = (List<CategoryModel>)serializer.Deserialize(reader);
                return new ObservableCollection<CategoryModel>(categories);
            }
        }


        public ObservableCollection<string> LoadRecipesNames()
        {
            ObservableCollection<string> fileNames = new();

            foreach (var name in Directory.GetFiles(recipesPath))
            {
                // .Split(".")[0] wycina końcówkę z rozszerzeniem
                fileNames.Add(Path.GetFileName(name).Split(".")[0]);
            }

            return fileNames;
        }

        public ObservableCollection<ProductModel> LoadProductsFromRecipe(string name)
        {
            var filePath = Path.Combine(recipesPath, name+".xml");

            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new(typeof(List<ProductModel>), new XmlRootAttribute("products"));

                using (StreamReader reader = new(filePath))
                {
                    List<ProductModel> products = (List<ProductModel>)serializer.Deserialize(reader);
                    CheckCategories(products);
                    CheckShops(products);
                    return new ObservableCollection<ProductModel>(products);
                }
            }else
            {
                return new ObservableCollection<ProductModel>();
            }
        }

        private void CheckCategories(List<ProductModel> productsToCheck)
        {
            foreach (var product in productsToCheck)
            {
                if (LoadCategories().Where(c => c.Name == product.Category).Count() == 0 )
                {
                    CategoryModel category = new();
                    category.Name = product.Category;
                    category.FillProducts();
                    AddCategory(category);
                }
            }
        }

        private void CheckShops(List<ProductModel> productsToCheck)
        {
            foreach (var product in productsToCheck)
            {
                if (LoadShops().Where(s => s.Name == product.Shop).Count() == 0)
                {
                    ShopModel shop = new();
                    shop.Name = product.Shop;
                    shop.FillProducts();
                    AddShop(shop);
                }
            }
        }

        private bool CheckProductsName(List<ProductModel> productToCheck)
        {
            foreach (var product in productToCheck)
            {
                if (LoadProducts().Where(p => p.Name == product.Name).Count() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckIfProductExist(string name)
        {
            foreach (var product in LoadProducts())
            {
                if (product.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckIfShopExist(string name)
        {
            foreach (var shop in LoadShops())
            {
                if (shop.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckIfRecipeExist(string name)
        {
            foreach (var recipe in LoadRecipesNames())
            {
                if (recipe == name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckIfCategoryExist(string name)
        {
            foreach (var category in LoadCategories())
            {
                if (category.Name == name)
                {
                    return true;
                }
            }

            return false; 
        }

        public ObservableCollection<ProductModel> ImportProducts(string path)
        {
            if (!File.Exists(path))
            {
                return new ObservableCollection<ProductModel>();
            }

            XmlSerializer serializer = new(typeof(List<ProductModel>), new XmlRootAttribute("products"));

            using (StreamReader reader = new(path))
            {
                List<ProductModel> products = (List<ProductModel>)serializer.Deserialize(reader);

                if (CheckProductsName(products))
                {
                    return new ObservableCollection<ProductModel>();
                }

                //sprawdz czy nie ma kategorii lub sklepu których nie ma w zasobach
                CheckCategories(products);
                CheckShops(products);

                foreach (var product in products)
                {
                    AddProduct(product);
                }

                return new ObservableCollection<ProductModel>(products);
            }
        }

        //ADD

        public void AddProduct(ProductModel newProduct)
        {
            var products = LoadProducts();
            if (!CheckIfProductExist(newProduct.Name))
            {
                products.Add(newProduct);
                SaveProducts(products);
            }
        }

        public void AddShop(ShopModel newShop)
        {
            var shops = LoadShops();
            if (!CheckIfShopExist(newShop.Name))
            {
                shops.Add(newShop);
                SaveShops(shops);
            }
        }

        public void AddCategory(CategoryModel newCategory)
        {
            var categories = LoadCategories();
            if (!CheckIfCategoryExist(newCategory.Name))
            {
                categories.Add(newCategory);
                SaveCategories(categories);
            }
        }

        //DELETE

        public void DeleteProductByName(string name)
        {
            XDocument doc = XDocument.Load(productsPath);

            var elementsToRemove = doc.Descendants("ProductModel").Where(e => (string)e.Attribute("Name") == name).ToList();

            foreach (var element in elementsToRemove)    
            {
                element.Remove();
            }

            doc.Save(productsPath);
        }

        public void DeleteRecipeByName(string name)
        {
            File.Delete(Path.Combine(recipesPath, name + ".xml"));
        }

        //UPDATE

        public void UpdateProduct(ProductModel product)
        {
            XDocument doc = XDocument.Load(productsPath);

            var elementsToUpdate = doc.Descendants("ProductModel").Where(e => (string)e.Attribute("Name") == product.Name).ToList();

            foreach (var element in elementsToUpdate)
            {
                //z założeń aplikacji tylko to może się zmienić
                element.SetAttributeValue("Quanity", product.Quanity);
                element.SetAttributeValue("BgColor", product.BgColor);
            }

            doc.Save(productsPath);
        }
    }

}

