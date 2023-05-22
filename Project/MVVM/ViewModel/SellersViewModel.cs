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
    public class SellersViewModel : ObservableObject
    {
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommandCheck<SellerDg> CheckBoxCommand { get; set; }

        private readonly ShineEntities _context;
        private ObservableCollection<SellerDg> _sellers;


        public List<SellerDg> SelectedSellers
        {
            get { return Sellers.Where(x => x.IsSelected).ToList(); }
        }


        public ObservableCollection<SellerDg> Sellers
        {
            get { return _sellers; }
            set
            {
                _sellers = value;
                OnPropertyChanged(nameof(Sellers));
            }
        }

        private SellerDg _selectedSeller;
        public SellerDg SelectedSeller
        {
            get => _selectedSeller;
            set
            {
                _selectedSeller = value;
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

        public SellersViewModel()
        {
            UpdateButtonVisibility();
            _context = new ShineEntities();
            RefreshSellers();
            AddCommand = new RelayCommand(o =>
            {
                var dialog = new AddSellerWindow();
                dialog.ShowDialog();
                RefreshSellers();
            });

            DeleteCommand = new RelayCommand(async o =>
            {
                if (SelectedSeller != null)
                {
                    await DeleteSellerAsync();
                }
                else
                    MessageBox.Show("Seler seller first");

            });

            EditCommand = new RelayCommand(async o =>
            {
                using (var context = new ShineEntities())
                {
                    var sellerToEdit = await context.Seller.FindAsync(SelectedSeller.Id);
                    var dialog = new EditSellerWindow(sellerToEdit);
                    dialog.ShowDialog();
                    RefreshSellers();
                }
            });


            RefreshCommand = new RelayCommand(o =>
            {
                RefreshSellers();
            });

            CheckBoxCommand = new RelayCommandCheck<SellerDg>(CheckBoxCommandExecute);
        }

        private void CheckBoxCommandExecute(SellerDg supply)
        {
            UpdateButtonVisibility();
        }

        private void RefreshSellers()
        {
            using (var context = new ShineEntities())
            {
                Sellers = new ObservableCollection<SellerDg>(
                    from s in context.Seller
                    select new SellerDg
                    {
                        Id = s.Id,
                        SellerName = s.SellerName,
                        Discount = s.Discount
                    });
            }
            UpdateButtonVisibility();
        }

        private void UpdateButtonVisibility()
        {
            int selectedCount = 0;

            if (Sellers != null)
            {
                selectedCount = SelectedSellers.Count();
            }

            IsAddCommandVisible = (selectedCount == 0);
            IsEditCommandVisible = (selectedCount == 1);
            IsDeleteCommandVisible = (selectedCount >= 1);
        }

        private async Task DeleteSellerAsync()
        {
            if (SelectedSellers.Count() > 0)
            {
                try
                {
                    using (var context = new ShineEntities())
                    {
                        foreach (var selseller in SelectedSellers)
                        {
                            var sellerToRemove = await context.Seller.FindAsync(selseller.Id);
                            context.Seller.Remove(sellerToRemove);
                        }
                        await context.SaveChangesAsync();
                        RefreshSellers();
                        SelectedSeller = null;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("This seller is already in use\n" +ex.Message);
                }
            }
            UpdateButtonVisibility();
        }
    }

    public class SellerDg
    {
        public int Id { get; set; }
        public string SellerName { get; set; }
        public double Discount { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
