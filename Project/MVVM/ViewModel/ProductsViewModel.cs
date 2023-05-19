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
    public class ProductDg
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public bool IsSelected { get; set; } = false;
    }
    public class ProductsViewModel : ObservableObject
    {
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommandCheck<ProductDg> CheckBoxCommand { get; set; }
        private readonly ShineEntities _context;
        private ObservableCollection<ProductDg> _products;

        public ObservableCollection<ProductDg> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public List<ProductDg> SelectedProducts
        {
            get { return Products.Where(x => x.IsSelected).ToList(); }
        }

        private ProductDg _selectedProduct;
        public ProductDg SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
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

        private void CheckBoxCommandExecute(ProductDg product)
        {
            UpdateButtonVisibility();
        }

        public ProductsViewModel()
        {
            _context = new ShineEntities();
            RefreshProducts();
            UpdateButtonVisibility();
            AddCommand = new RelayCommand(o =>
            {
                var dialog = new AddProductWindow();
                dialog.ShowDialog();
                RefreshProducts();
            });

            DeleteCommand = new RelayCommand(async o =>
            {
                await DeleteProductAsync();
            });

            EditCommand = new RelayCommand(async o =>
            {
                using (var context = new ShineEntities())
                {
                    var productToEdit = await context.Product.FindAsync(SelectedProduct.Id);
                    var dialog = new EditProductWindow(productToEdit);
                    dialog.ShowDialog();
                    RefreshProducts();
                }
            });

            RefreshCommand = new RelayCommand(o =>
            {
                RefreshProducts();
            });

            CheckBoxCommand = new RelayCommandCheck<ProductDg>(CheckBoxCommandExecute);
        }

        private void RefreshProducts()
        {
            using (var context = new ShineEntities())
            {
                Products = new ObservableCollection<ProductDg>(
                    from p in context.Product
                    select new ProductDg
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        Price = p.Price
                    });
            }
        }


        private void UpdateButtonVisibility()
        {
            int selectedCount = 0;

            if (Products != null)
            {
                selectedCount = SelectedProducts.Count();
            }

            IsAddCommandVisible = (selectedCount == 0);
            IsEditCommandVisible = (selectedCount == 1);
            IsDeleteCommandVisible = (selectedCount >= 1);
        }

        private async Task DeleteProductAsync()
        {
            if (SelectedProducts.Count > 0)
            {
                using (var context = new ShineEntities())
                {
                    foreach (var selproduct in SelectedProducts)
                    {
                        var productToRemove = await context.Product.FindAsync(selproduct.Id);
                        context.Product.Remove(productToRemove);
                    }
                    await context.SaveChangesAsync();
                    RefreshProducts();
                    SelectedProduct = null;
                }
            }
            UpdateButtonVisibility();
        }
    }

}
