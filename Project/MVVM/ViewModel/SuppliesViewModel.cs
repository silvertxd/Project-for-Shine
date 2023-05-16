using Project.AddWindows;
using Project.Core;
using Project.EditWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;


namespace Project.MVVM.ViewModel
{
    public class SuppliesViewModel : ObservableObject
    {
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        private readonly ShineEntities _context;
        private ObservableCollection<Supply> _supplies;
        private ObservableCollection<SupplyDg> _displaySupplies;
        public ObservableCollection<SupplyDg> DisplaySupplies
        {
            get => _displaySupplies;
            set
            {
                _displaySupplies = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Supply> Supplies
        {
            get => _supplies; 
            set
            {
                _supplies = value;
                OnPropertyChanged();
            }
        }


        private SupplyDg _selectedSupply;
        public SupplyDg SelectedSupply
        {
            get => _selectedSupply;
            set
            {
                _selectedSupply = value;
                OnPropertyChanged();
            }
        }

        public SuppliesViewModel()
        {
            _context = new ShineEntities();
            DisplaySupplies = new ObservableCollection<SupplyDg>(
                from s in _context.Supply
                join p in _context.Product on s.ProductId equals p.Id
                join sl in _context.Seller on s.SellerId equals sl.Id
                select new SupplyDg
                {
                    Id = s.Id,
                    SupplyDate = s.SupplyDate,
                    ProductName = p.ProductName,
                    Quantity = s.Quantity,
                    SellerName = sl.SellerName,
                    TotalPrice = s.TotalPrice
                });
            AddCommand = new RelayCommand(o =>
            {
                var dialog = new AddSupplyWindow();
                dialog.ShowDialog();
                RefreshSuppliesAsync();
            });
            DeleteCommand = new RelayCommand(async o =>
            {
                if (SelectedSupply != null)
                {
                    await DeleteSupplyAsync();
                }
                else
                    MessageBox.Show("Seler seller first");
            });
            EditCommand = new RelayCommand(async o =>
            {
                if(SelectedSupply!= null)
                {
                    using (var context = new ShineEntities())
                    {
                        var supplyToEdit = await context.Supply.FindAsync(SelectedSupply.Id);
                        var dialog = new EditSupplyWindow(supplyToEdit);
                        dialog.ShowDialog();
                        RefreshSuppliesAsync();
                    }
                }
                else
                    MessageBox.Show("Seler supply first");
            });
        }

        private async Task DeleteSupplyAsync()
        {
            if (SelectedSupply != null)
            {
                using (var context = new ShineEntities())
                {
                    var supplyToRemove = await context.Supply.FindAsync(SelectedSupply.Id);
                    context.Supply.Remove(supplyToRemove);
                    await context.SaveChangesAsync();
                    RefreshSuppliesAsync();
                    SelectedSupply = null;
                }
            }
        }

        private void RefreshSuppliesAsync()
        {
            DisplaySupplies = new ObservableCollection<SupplyDg>(
                from s in _context.Supply
                join p in _context.Product on s.ProductId equals p.Id
                join sl in _context.Seller on s.SellerId equals sl.Id
                select new SupplyDg
                {
                    Id = s.Id,
                    SupplyDate = s.SupplyDate,
                    ProductName = p.ProductName,
                    Quantity = s.Quantity,
                    SellerName = sl.SellerName,
                    TotalPrice = s.TotalPrice
                });
        }
    }
}
