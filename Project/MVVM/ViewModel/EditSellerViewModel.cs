using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Project.Core;

namespace Project.MVVM.ViewModel
{
    public class EditSellerViewModel : ObservableObject
    {
        public RelayCommand SaveCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand CancelCommand { get; set; }
        private Seller _seller;

        public Seller Seller
        {
            get => _seller;
            set
            {
                _seller = value;
                OnPropertyChanged();
            }

        }

        private int _discount;

        public int Discount
        {
            get { return _discount; }
            set 
            { 
                _discount = value; 
                OnPropertyChanged();
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




        public EditSellerViewModel() { }


        public EditSellerViewModel(Seller seller)
        {
            Seller = seller; 
            SaveCommand = new RelayCommand(o =>
            {
                
                
            });
            CancelCommand = new RelayCommand(o =>
            {
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                window?.Close();
            });


        }


    }
}
