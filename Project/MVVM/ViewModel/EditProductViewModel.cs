using System;
using System.Linq;
using System.Windows;
using Project.Core;

namespace Project.MVVM.ViewModel
{
    public class EditProductViewModel : ObservableObject
    {
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        private Product _product;

        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        private double _price;

        public double Price
        {
            get 
            {
                return _price;
            }
            set 
            { 
                OnPropertyChanged();
                _price = value;
            }
        }

        private string _productname;

        public string ProductName
        {
            get
            {
                return _productname;
            }
            set
            {
                _productname = value;
                OnPropertyChanged();
            }
        }

        public EditProductViewModel(Product product)
        {
            Product = product;
            SaveCommand = new RelayCommand(async o =>
            {
                try
                {
                    using (var db = new ShineEntities())
                    {
                        var newProduct = db.Product.Find(Product.Id);
                        newProduct.ProductName = _productname;
                        newProduct.Price = _price;
                        await db.SaveChangesAsync();
                        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                        window?.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }


            });
            CancelCommand = new RelayCommand(o =>
            {
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                window?.Close();
            });
        }

    }
}
