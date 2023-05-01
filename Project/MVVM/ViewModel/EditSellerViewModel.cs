using System;
using System.Collections.Generic;
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

        public EditSellerViewModel(Seller seller)
        {
            SaveCommand = new RelayCommand(o =>
            {
                var newSeller = seller;
                
            });
        }


    }
}
