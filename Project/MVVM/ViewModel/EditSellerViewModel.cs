﻿using System;
using System.Linq;
using System.Windows;
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

        private double _discount;

        public double Discount
        {
            get
            {
                return _discount;
            }
            set 
            {
                OnPropertyChanged();
                _discount = value;
            }
        }

        public double DiscountText
        {
            get 
            {
                return _discount*100;  
            }
            set
            {
                _discount = value;
                OnPropertyChanged();
            }
        }

        private string _sellername;

        public string SellerName
        {
            get { return _sellername; }
            set 
            {
                _sellername = value;
                OnPropertyChanged();
            }
        }


        public EditSellerViewModel(Seller seller)
        {
            Seller = seller; 
            SaveCommand = new RelayCommand(async o =>
            {
                try
                {
                    using (var db = new ShineEntities())
                    {
                        var newSeller = db.Seller.Find(Seller.Id);
                        newSeller.SellerName = _sellername;
                        if(_discount > 1)
                            newSeller.Discount = _discount / 100;
                        else
                            newSeller.Discount = _discount;
                        await db.SaveChangesAsync();
                        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                        window?.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }


            });
            CancelCommand = new RelayCommand(o =>
            {
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                window?.Close();
            });


        }


    }
}
