using Project.MVVM.ViewModel;
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
        public Seller EditSeller { get; set; }
        public EditSellerWindow(Seller seller)
        {
            InitializeComponent();
            txtBoxSellerName.Text = seller.SellerName;
            numericUpdownDiscount.Value = Convert.ToInt32(seller.Discount * 100);
            EditSeller = seller;
            var EditSellerVM = new EditSellerViewModel(seller);
        }
    }
}
