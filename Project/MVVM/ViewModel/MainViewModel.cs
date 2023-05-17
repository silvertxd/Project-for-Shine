using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Project.Core;
namespace Project.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand MinimizeCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand OrdersViewCommand { get; set; }
        public RelayCommand SuppliesViewCommand { get; set; }
        public RelayCommand SellersViewCommand { get; set; }
        public RelayCommand ExportViewCommand { get; set; }

        public SuppliesViewModel OrdersVM { get; set; }
        public ProductsViewModel SuppliesVM { get; set; }
        public SellersViewModel SellersVM { get; set; }
        public ExportViewModel ExportVM { get; set; }

        private string selectedLanguage;
        public string SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
            }
        }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }




        public MainViewModel()
        {
            OrdersVM = new SuppliesViewModel();
            SuppliesVM = new ProductsViewModel();
            SellersVM = new SellersViewModel();
            ExportVM = new ExportViewModel();
            CurrentView = OrdersVM;


            OrdersViewCommand = new RelayCommand(o =>
            {
                CurrentView = OrdersVM;
            });

            SuppliesViewCommand = new RelayCommand(o =>
            {
                CurrentView = SuppliesVM;
            });

            SellersViewCommand = new RelayCommand(o =>
            {
                CurrentView = SellersVM;
            });

            ExportViewCommand = new RelayCommand(o =>
            {
                CurrentView = ExportVM;
            });

            CloseCommand = new RelayCommand(o =>
            {
                Application.Current.MainWindow.Close();
            });

            MinimizeCommand = new RelayCommand(o =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
            
        }
    }
}
