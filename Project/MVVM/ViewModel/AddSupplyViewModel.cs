using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class AddSupplyViewModel : ObservableObject
    {
        public RelayCommand AddCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

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
            set { 
                OnPropertyChanged();
                _sellerId = value; 
            }
        }

        private int _quantity;

        public int Quantity
        {
            get => _quantity; 
            set {
                OnPropertyChanged();
                _quantity = value; 
            }
        }

        private DateTime _supplydate = DateTime.Now;

        public DateTime SupplyDate
        {
            get => _supplydate; 
            set 
            {
                OnPropertyChanged();
                _supplydate = value; 
            }

        }

        public AddSupplyViewModel()
        {
            using (var db = new ShineEntities())
            {
                Sellers = db.Seller.ToList();
                Products = db.Product.ToList();
            }


            AddCommand = new RelayCommand(async o =>
            {
                using (var db = new ShineEntities())
                {
                    double price = db.Product.Find(ProductId).Price;
                    double discount = db.Seller.Find(SellerId).Discount;
                    double discPrice = price-(price*discount);
                    double fullPrice = discPrice * Quantity;
                    db.Supply.Add(new Supply { ProductId = ProductId, SellerId = SellerId, SupplyDate = SupplyDate,
                    Quantity = Quantity, TotalPrice = (double)fullPrice});
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
