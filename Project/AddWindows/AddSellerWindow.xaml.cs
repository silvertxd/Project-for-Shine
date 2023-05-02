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

namespace Project.AddWindows
{
    /// <summary>
    /// Interaction logic for AddSeller.xaml
    /// </summary>
    public partial class AddSellerWindow : Window
    {
        public AddSellerWindow()
        {
            InitializeComponent();
            var viewModelAdd = new AddSellerViewModel();
            DataContext = viewModelAdd;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}