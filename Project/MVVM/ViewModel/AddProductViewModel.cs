using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Project.Core;

namespace Project.MVVM.ViewModel
{
    public class AddProductViewModel : ObservableObject
    {
        public RelayCommand AddCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

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

        private string _productName;

        public string ProductName
        {
            get
            {
                return _productName;
            }
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        public AddProductViewModel()
        {
            AddCommand = new RelayCommand(async o =>
            {
                if (string.IsNullOrEmpty(_productName))
                {
                    MessageBox.Show("Some of fields are empty");
                }
                else
                {
                    using (var db = new ShineEntities())
                    {
                        db.Product.Add(new Product { ProductName = _productName, Price = _price });
                        await db.SaveChangesAsync();
                        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                        window?.Close();
                    }
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
