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
    public class SellersViewModel : ObservableObject
    {
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        private readonly ShineEntities _context;
        private ObservableCollection<Seller> _sellers;


    


        public ObservableCollection<Seller> Sellers
        {
            get { return _sellers; }
            set
            {
                _sellers = value;
                OnPropertyChanged(nameof(Sellers));
            }
        }

        private Seller _selectedSeller;
        public Seller SelectedSeller
        {
            get => _selectedSeller;
            set
            {
                _selectedSeller = value;
                OnPropertyChanged();
            }
        }

        public SellersViewModel()
        {
            _context = new ShineEntities();
            LoadData();
            AddCommand = new RelayCommand(o =>
            {
                MessageBox.Show("123");
            });

            DeleteCommand = new RelayCommand(async o =>
            {
                if (SelectedSeller != null)
                {
                    await DeleteSellerAsync();
                }
            });

            EditCommand = new RelayCommand(async o =>
            {
                if (SelectedSeller != null)
                {
                    using (var context = new ShineEntities())
                    {
                        var sellerToEdit = await context.Seller.FindAsync(SelectedSeller.Id);
                        var dialog = new EditSellerWindow(sellerToEdit);
                        dialog.ShowDialog();
                    }
                    
                    
                }
            });


            RefreshCommand = new RelayCommand(async o =>
            {
                await RefreshSellersAsync();
            });
        }

        private async Task RefreshSellersAsync()
        {
            using (var context = new ShineEntities())
            {
                Sellers = new ObservableCollection<Seller>(await context.Seller.ToListAsync());
            }
        }

        private void LoadData()
        {
            Sellers = new ObservableCollection<Seller>(_context.Seller.ToList());
        }

        private async Task DeleteSellerAsync()
        {
            if (SelectedSeller != null)
            {
                using (var context = new ShineEntities())
                {
                    var sellerToRemove = await context.Seller.FindAsync(SelectedSeller.Id);
                    context.Seller.Remove(sellerToRemove);
                    await context.SaveChangesAsync();
                    await RefreshSellersAsync();
                    SelectedSeller = null;
                }
            }
        }
    }
}
