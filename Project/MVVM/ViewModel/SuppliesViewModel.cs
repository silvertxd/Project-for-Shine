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
using System.Windows.Controls;
using System.Windows.Input;

namespace Project.MVVM.ViewModel
{
    public class SuppliesViewModel : ObservableObject
    {
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommandCheck<SupplyDg> CheckBoxCommand { get; set; }
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


        public List<SupplyDg> SelectedSupplies
        {
            get { return DisplaySupplies.Where(x => x.IsSelected).ToList(); }
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

        private bool _isAddCommandVisible;
        public bool IsAddCommandVisible
        {
            get => _isAddCommandVisible;
            set
            {
                _isAddCommandVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isEditCommandVisible;
        public bool IsEditCommandVisible
        {
            get => _isEditCommandVisible;
            set
            {
                _isEditCommandVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isDeleteCommandVisible;
        public bool IsDeleteCommandVisible
        {
            get => _isDeleteCommandVisible;
            set
            {
                _isDeleteCommandVisible = value;
                OnPropertyChanged();
            }
        }

        public SuppliesViewModel()
        {
            UpdateButtonVisibility();
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
                        var supplyToEdit = await context.Supply.FindAsync(SelectedSupplies.FirstOrDefault().Id);
                        var dialog = new EditSupplyWindow(supplyToEdit);
                        dialog.ShowDialog();
                        RefreshSuppliesAsync();
                    }
                }
                
                else
                    MessageBox.Show("Seler supply first");
            });
            RefreshCommand = new RelayCommand(o =>
            {
                RefreshSuppliesAsync();
            });
            CheckBoxCommand = new RelayCommandCheck<SupplyDg>(CheckBoxCommandExecute);
        }

        private void CheckBoxCommandExecute(SupplyDg supply)
        {
            UpdateButtonVisibility();
        }

        private async Task DeleteSupplyAsync()
        {
            if (SelectedSupplies.Count > 0)
            {
                using (var context = new ShineEntities())
                {
                    foreach(var selsupply in SelectedSupplies)
                    {
                        var supplyToRemove = await context.Supply.FindAsync(selsupply.Id);
                        context.Supply.Remove(supplyToRemove);
                    }
                    await context.SaveChangesAsync();
                    RefreshSuppliesAsync();
                    SelectedSupply = null;
                }
            }
            UpdateButtonVisibility();
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
            UpdateButtonVisibility();
        }


        private void UpdateButtonVisibility()
        {
            int selectedCount = 0;

            if (DisplaySupplies != null)
            {
                selectedCount = SelectedSupplies.Count();
            }

            IsAddCommandVisible = (selectedCount == 0);
            IsEditCommandVisible = (selectedCount == 1);
            IsDeleteCommandVisible = (selectedCount >= 1);
        }
    }

    public class RelayCommandCheck<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommandCheck(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommandCheck(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
