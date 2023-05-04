using Project.AddWindows;
using Project.Core;
using Project.EditWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.MVVM.ViewModel
{
    public class ProductsViewModel : ObservableObject
    {
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        private readonly ShineEntities _context;
        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct; 
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public ProductsViewModel()
        {
            _context = new ShineEntities();
            LoadData();
            AddCommand = new RelayCommand(async o =>
            {
                var dialog = new AddProductWindow();
                dialog.ShowDialog();
                await RefreshProductsAsync();
            });

            DeleteCommand = new RelayCommand(async o =>
            {
                if (SelectedProduct != null)
                {
                    await DeleteProductAsync();
                }
                else
                    MessageBox.Show("Seler product first");
            });

            EditCommand = new RelayCommand(async o =>
            {
                if (SelectedProduct != null)
                {
                    using (var context = new ShineEntities())
                    {
                        var productToEdit = await context.Product.FindAsync(SelectedProduct.Id);
                        var dialog = new EditProductWindow(productToEdit);
                        dialog.ShowDialog();
                        await RefreshProductsAsync();
                    }
                }
                else
                    MessageBox.Show("Seler seller first");
            });

            RefreshCommand = new RelayCommand(async o =>
            {
                await RefreshProductsAsync();
            });
        }

        private async Task RefreshProductsAsync()
        {
            using (var context = new ShineEntities())
            {
                Products = new ObservableCollection<Product>(await context.Product.ToListAsync());
            }
        }


        private void LoadData()
        {
            Products = new ObservableCollection<Product>(_context.Product.ToList());
        }

        private async Task DeleteProductAsync()
        {
            if (SelectedProduct != null)
            {
                using (var context = new ShineEntities())
                {
                    var productToRemove = await context.Product.FindAsync(SelectedProduct.Id);
                    context.Product.Remove(productToRemove);
                    await context.SaveChangesAsync();
                    await RefreshProductsAsync();
                    SelectedProduct = null;
                }
            }
        }
    }
}
