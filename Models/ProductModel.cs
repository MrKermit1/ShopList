using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace ShopList.Models
{
    public class ProductModel : INotifyPropertyChanged
    {

        private string _name = String.Empty;
        private string _shop = String.Empty;
        private string _unit = String.Empty;
        private string _category = String.Empty;
        private int _quanity;
        private bool _check;
        private bool _optional;
        private string _bgcolor =String.Empty;


        [XmlAttribute("Name")]
        public string Name 
        {
            get => _name;
            
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        
        [XmlAttribute("Shop")]
        public string Shop 
        {
            get => _shop;
            
            set
            {
                _shop = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute("Unit")]
        public string Unit 
        {
            get => _unit;

            set
            {
                _unit = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute("Quanity")]
        public int Quanity 
        { 
            get => _quanity;

            set
            {
                _quanity = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute("Check")]
        public bool Check 
        {
            get => _check;
            set
            {
                _check = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute("BgColor")]
        public string BgColor
        {
            get => _bgcolor;

            set
            {
                _bgcolor = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute("Category")]
        public string Category
        {
            get => _category;

            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute("Optional")]
        public bool Optional
        {
            get => _optional;

            set
            {
                _optional = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Increment()
        {
            Quanity++;
        }

        public void Decrement()
        {
            Quanity--;
        }
    }
}
