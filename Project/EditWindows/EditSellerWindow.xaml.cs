﻿using Project.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project.EditWindows
{
    /// <summary>
    /// Interaction logic for EditSellerWindow.xaml
    /// </summary>
    public partial class EditSellerWindow : Window
    {
        public EditSellerWindow(Seller seller)
        {
            InitializeComponent();
            var viewModelEdit = new EditSellerViewModel(seller);
            viewModelEdit.SellerName = seller.SellerName;
            viewModelEdit.Discount = seller.Discount;

            DataContext = viewModelEdit;
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

    }
}
