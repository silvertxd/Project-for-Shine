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
    public class AddSellerViewModel : ObservableObject
    {
        public RelayCommand AddCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        private double _discount;

        public double Discount
        {
            get
            {
                return _discount;
            }
            set
            {
                OnPropertyChanged();
                _discount = value;
            }
        }

        private string _sellername;

        public string SellerName
        {
            get { return _sellername; }
            set
            {
                _sellername = value;
                OnPropertyChanged();
            }
        }

        public AddSellerViewModel()
        {
            AddCommand = new RelayCommand(async o =>
            {
                if(string.IsNullOrEmpty(_sellername) || _discount == 0)
                {
                    MessageBox.Show("Some of fields are empty");
                }
                else
                {
                    using (var db = new ShineEntities())
                    {
                        db.Seller.Add(new Seller { SellerName = _sellername, Discount = _discount/100 });
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
