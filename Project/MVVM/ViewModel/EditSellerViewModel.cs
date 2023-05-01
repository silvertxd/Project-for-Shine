using Project.Core;
using Project.EditWindows;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;

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
            SaveCommand = new RelayCommand(async o =>
            {
                var newSeller = seller;
                
            });
        }


    }
}
