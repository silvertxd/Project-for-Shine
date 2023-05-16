using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Project.Core;

namespace Project.MVVM.ViewModel
{
    public class EditSupplyViewModel : ObservableObject
    {
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        private Supply _supply;

        private Supply _selectedSupply;

        public Supply SelectedSupply
        {
            get => _selectedSupply;
            set
            {
                OnPropertyChanged();
                _selectedSupply = value;
            }
        }

        public Supply Supply
        {
            get => _supply; 
            set
            {
                _supply = value;
                OnPropertyChanged();
            }
        }

        private List<Seller> _sellers;
        public List<Seller> Sellers
        {
            get => _sellers;
            set
            {
                _sellers = value;
                OnPropertyChanged();
            }
        }

        private List<Product> _products;
        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }


        private int _productId;

        public int ProductId
        {
            get => _productId;
            set
            {
                OnPropertyChanged();
                _productId = value;
            }
        }

        private int _sellerId;

        public int SellerId
        {
            get => _sellerId;
            set
            {
                OnPropertyChanged();
                _sellerId = value;
            }
        }

        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                OnPropertyChanged();
                _quantity = value;
            }
        }

        private DateTime _supplydate;

        public DateTime SupplyDate
        {
            get => _supplydate;
            set
            {
                OnPropertyChanged();
                _supplydate = value;
            }

        }





        public EditSupplyViewModel(Supply supply)
        {
            Supply = supply;
            using (var db = new ShineEntities())
            {
                Sellers = db.Seller.ToList();
                Products = db.Product.ToList();
            }
            SaveCommand = new RelayCommand(async o =>
            {
                using(var db = new ShineEntities())
                {
                    var newSupply = db.Supply.Find(Supply.Id);
                    newSupply.Quantity = _quantity;
                    newSupply.SupplyDate = _supplydate;
                    newSupply.SellerId = _sellerId;
                    newSupply.ProductId = _productId;
                    double price = db.Product.Find(ProductId).Price;
                    double discount = db.Seller.Find(SellerId).Discount;
                    double discPrice = price - (price * discount);
                    double fullPrice = discPrice * Quantity;
                    newSupply.TotalPrice = fullPrice;
                    await db.SaveChangesAsync();
                    var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                    window?.Close();
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
