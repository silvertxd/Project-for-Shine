using Project.EditWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using (var context = new ShineEntities())
            {
                var supplyData = from s in context.Supply
                                 join p in context.Product on s.ProductId equals p.Id
                                 join sl in context.Seller on s.SellerId equals sl.Id
                                 select new
                                 {
                                     s.Id,
                                     s.SupplyDate,
                                     ProductName = p.ProductName,
                                     s.Quantity,
                                     SellerName = sl.SellerName,
                                     s.TotalPrice
                                 };
                
            }
            

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
