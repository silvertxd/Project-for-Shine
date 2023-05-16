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
    /// Interaction logic for EditSupplyWindow.xaml
    /// </summary>
    public partial class EditSupplyWindow : Window
    {
        public EditSupplyWindow(Supply supply)
        {
            InitializeComponent();
            var viewModelEdit = new EditSupplyViewModel(supply);
            viewModelEdit.Quantity = supply.Quantity;
            viewModelEdit.SellerId = supply.SellerId;
            viewModelEdit.ProductId = supply.ProductId;
            viewModelEdit.SupplyDate = supply.SupplyDate;
            viewModelEdit.SelectedSupply = supply;
            DataContext = viewModelEdit;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
